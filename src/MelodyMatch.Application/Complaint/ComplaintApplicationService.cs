using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MelodyMatch.Complaint.DTOs.Requests;
using MelodyMatch.Complaint.DTOs.Responses;
using MelodyMatch.Complaint.Filters;
using MelodyMatch.Complaint.Services;
using MelodyMatch.Complaints;
using MelodyMatch.Constants;
using MelodyMatch.Enums.Complaints;
using MelodyMatch.Extensions;
using MelodyMatch.Notification.Services;
using MelodyMatch.Services;
using MelodyMatch.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MelodyMatch.Complaint;

[Authorize(Roles = RolesConsts.Admin + "," + RolesConsts.Dater)]
public class ComplaintApplicationService : ApplicationService, IComplaintApplicationService
{
    private readonly IComplaintRepository _complaintRepository;
    private readonly UserBanService _userBanService;
    private readonly IMelodyMatchUserRepository _userRepository;
    private readonly NotificationSenderService _notificationSenderService;
    
    public ComplaintApplicationService(
        IComplaintRepository complaintRepository,
        UserBanService userBanService,
        IMelodyMatchUserRepository userRepository,
        NotificationSenderService notificationSenderService)
    {
        _complaintRepository = complaintRepository;
        _userBanService = userBanService;
        _userRepository = userRepository;
        _notificationSenderService = notificationSenderService;
    }

    public async Task<PagedResultDto<ComplaintResponseDto>> GetListAsync(ComplaintFilter filter)
    {
        var query = await _complaintRepository.GetQueryableAsync();

        query = query
            .FilterBy(filter.ReporterId != null, x => x.ReporterId == filter.ReporterId)
            .FilterBy(filter.ReportedUserId != null, x => x.ReportedUserId == filter.ReportedUserId)
            .FilterBy(filter.Reason != null, x => x.Reason.Contains(filter.Reason!))
            .FilterBy(filter.Status != null, x => x.Status == filter.Status)
            .FilterBy(filter.CreatedAtFrom != null, x => x.CreationTime >= filter.CreatedAtFrom)
            .FilterBy(filter.CreatedAtTo != null, x => x.CreationTime <= filter.CreatedAtTo);
        
        var totalCount = await AsyncExecuter.CountAsync(query);
        
        query = query
            .OrderByDescending(x => x.CreationTime)
            .Skip(filter.SkipCount)
            .Take(filter.MaxResultCount);
        
        var complaints = await AsyncExecuter.ToListAsync(
            query
                .Include(x => x.Reporter)
                    .ThenInclude(x => x.UserProfile)
                .Include(x => x.ReportedUser)
                    .ThenInclude(x => x.UserProfile));
        
        return new PagedResultDto<ComplaintResponseDto>(
            totalCount,
            ObjectMapper.Map<List<Complaints.Complaint>, List<ComplaintResponseDto>>(complaints));
    }

    public async Task<ComplaintResponseDto> GetByIdAsync(Guid id)
    {
        var complaint = await _complaintRepository.GetByIdAsync(id);
        
        return ObjectMapper.Map<Complaints.Complaint, ComplaintResponseDto>(complaint);
    }

    public async Task<ComplaintResponseDto> AddComplaintAsync(CreateComplaintRequestDto request)
    {
        var complaint = ObjectMapper.Map<CreateComplaintRequestDto, Complaints.Complaint>(request);
        var createdComplaint = await _complaintRepository.AddComplaintAsync(complaint);
        
        return ObjectMapper.Map<Complaints.Complaint, ComplaintResponseDto>(createdComplaint);
    }

    public async Task<ComplaintResponseDto> UpdateComplaintAsync(UpdateComplaintRequestDto request)
    {
        var complaint = await _complaintRepository.GetByIdAsync(request.Id);
        ObjectMapper.Map(request, complaint);
        var updatedComplaint = await _complaintRepository.UpdateAsync(complaint);
        
        return ObjectMapper.Map<Complaints.Complaint, ComplaintResponseDto>(updatedComplaint);
    }

    [Authorize(Roles = RolesConsts.Admin)]
    public async Task DeleteComplaintAsync(Guid id)
    {
        var complaint = await _complaintRepository.GetByIdAsync(id);
        await _complaintRepository.DeleteAsync(complaint);
    }

    [Authorize(Roles = RolesConsts.Admin)]
    public async Task<ComplaintResponseDto> BanUserFromComplaintAsync(BanUserFromComplaintRequestDto request)
    {
        var complaint = await _complaintRepository.GetByIdAsync(request.ComplaintId);
        
        if (complaint == null)
        {
            throw new UserFriendlyException("Скарга не знайдена");
        }
        
        if (complaint.Status == ComplaintStatus.Resolved)
        {
            throw new UserFriendlyException("Ця скарга вже розглянута");
        }

        // Ban the reported user
        await _userBanService.BanUserAsync(
            complaint.ReportedUserId,
            request.BanReason,
            complaint.Id,
            request.ExpiresAt,
            request.IsPermanent);

        // Update complaint status
        complaint.Status = ComplaintStatus.Resolved;
        await _complaintRepository.UpdateAsync(complaint);

        // Get reported user info
        var reportedUser = await _userRepository.GetByIdAsync(complaint.ReportedUserId);
        var reportedUserName = reportedUser.IdentityUser.UserName ?? "Unknown";

        // Create and send notification to reporter
        var notification = await _userBanService.CreateComplaintResolutionNotificationAsync(
            complaint.ReporterId,
            complaint.Id,
            true,
            reportedUserName,
            request.BanReason);
        
        // Send real-time notification via SignalR
        var notificationDto = ObjectMapper.Map<Notifications.Notification, MelodyMatch.Notification.DTOs.Responses.NotificationResponseDto>(notification);
        await _notificationSenderService.SendNotificationToUserAsync(complaint.ReporterId, notificationDto);

        return ObjectMapper.Map<Complaints.Complaint, ComplaintResponseDto>(complaint);
    }

    [Authorize(Roles = RolesConsts.Admin)]
    public async Task<ComplaintResponseDto> ResolveComplaintAsync(ResolveComplaintRequestDto request)
    {
        var complaint = await _complaintRepository.GetByIdAsync(request.ComplaintId);
        
        if (complaint == null)
        {
            throw new UserFriendlyException("Скарга не знайдена");
        }
        
        if (complaint.Status == ComplaintStatus.Resolved || complaint.Status == ComplaintStatus.Dismissed)
        {
            throw new UserFriendlyException("Ця скарга вже розглянута");
        }

        // Update complaint status
        complaint.Status = request.Status;
        await _complaintRepository.UpdateAsync(complaint);

        // Get reported user info
        var reportedUser = await _userRepository.GetByIdAsync(complaint.ReportedUserId);
        var reportedUserName = reportedUser.IdentityUser.UserName ?? "Unknown";

        // Create and send notification to reporter (user was pardoned)
        var notification = await _userBanService.CreateComplaintResolutionNotificationAsync(
            complaint.ReporterId,
            complaint.Id,
            false,
            reportedUserName);
        
        // Send real-time notification via SignalR
        var notificationDto = ObjectMapper.Map<Notifications.Notification, MelodyMatch.Notification.DTOs.Responses.NotificationResponseDto>(notification);
        await _notificationSenderService.SendNotificationToUserAsync(complaint.ReporterId, notificationDto);

        return ObjectMapper.Map<Complaints.Complaint, ComplaintResponseDto>(complaint);
    }
}