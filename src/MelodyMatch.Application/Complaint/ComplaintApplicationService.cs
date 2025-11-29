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
using MelodyMatch.Enums.Notifications;
using MelodyMatch.Extensions;
using MelodyMatch.Notification.Services;
using MelodyMatch.Notifications;
using MelodyMatch.Services;
using MelodyMatch.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace MelodyMatch.Complaint;

[Authorize(Roles = RolesConsts.Admin + "," + RolesConsts.Dater)]
public class ComplaintApplicationService : ApplicationService, IComplaintApplicationService
{
    private readonly IComplaintRepository _complaintRepository;
    private readonly UserBanService _userBanService;
    private readonly IMelodyMatchUserRepository _userRepository;
    private readonly NotificationSenderService _notificationSenderService;
    private readonly INotificationRepository _notificationRepository;
    private readonly IdentityUserManager _identityUserManager;
    private readonly IdentityRoleManager _identityRoleManager;
    
    public ComplaintApplicationService(
        IComplaintRepository complaintRepository,
        UserBanService userBanService,
        IMelodyMatchUserRepository userRepository,
        NotificationSenderService notificationSenderService,
        INotificationRepository notificationRepository,
        IdentityUserManager identityUserManager,
        IdentityRoleManager identityRoleManager)
    {
        _complaintRepository = complaintRepository;
        _userBanService = userBanService;
        _userRepository = userRepository;
        _notificationSenderService = notificationSenderService;
        _notificationRepository = notificationRepository;
        _identityUserManager = identityUserManager;
        _identityRoleManager = identityRoleManager;
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
                .Include(x => x.Reporter)
                    .ThenInclude(x => x.IdentityUser)
                .Include(x => x.ReportedUser)
                    .ThenInclude(x => x.UserProfile)
                        .ThenInclude(x => x.ProfilePhotos!)
                .Include(x => x.ReportedUser)
                    .ThenInclude(x => x.IdentityUser));
        
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
        
        var complaintWithProfile = await _complaintRepository.GetByIdAsync(createdComplaint.Id);
        
        var complaintDto = ObjectMapper.Map<Complaints.Complaint, ComplaintResponseDto>(complaintWithProfile);
        
        var adminRole = await _identityRoleManager.FindByNameAsync(RolesConsts.Admin);
        if (adminRole != null)
        {
            var adminsInRole = await _identityUserManager.GetUsersInRoleAsync(RolesConsts.Admin);
            
            foreach (var adminIdentityUser in adminsInRole)
            {
                var adminUser = await _userRepository.GetByIdentityUserIdAsync(adminIdentityUser.Id);
                if (adminUser != null)
                {
                    var reportedUserName = complaintWithProfile.ReportedUser?.IdentityUser?.UserName ?? "Невідомий користувач";
                    var reporterName = complaintWithProfile.Reporter?.IdentityUser?.UserName ?? "Невідомий користувач";
                    
                    var notification = new Notifications.Notification(
                        adminUser.Id,
                        NotificationType.ComplaintReceived,
                        "Нова скарга",
                        $"Користувач {reporterName} поскаржився на {reportedUserName}. Причина: {complaintWithProfile.Reason}",
                        complaintWithProfile.Id);
                    
                    var createdNotification = await _notificationRepository.InsertAsync(notification);
                    
                    var notificationDto = ObjectMapper.Map<Notifications.Notification, MelodyMatch.Notification.DTOs.Responses.NotificationResponseDto>(createdNotification);
                    await _notificationSenderService.SendNotificationToUserAsync(adminUser.Id, notificationDto);
                }
            }
        }
        
        return complaintDto;
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
            throw new UserFriendlyException("Complaint not found");
        }
        
        if (complaint.Status == ComplaintStatus.Resolved)
        {
            throw new UserFriendlyException("This complain was reviewed");
        }

        await _userBanService.BanUserAsync(
            complaint.ReportedUserId,
            request.BanReason,
            complaint.Id,
            null,
            true);

        complaint.Status = ComplaintStatus.Resolved;
        await _complaintRepository.UpdateAsync(complaint);

        var reportedUser = await _userRepository.GetByIdAsync(complaint.ReportedUserId);
        var reportedUserName = reportedUser.IdentityUser.UserName ?? "Unknown";

        var notification = await _userBanService.CreateComplaintResolutionNotificationAsync(
            complaint.ReporterId,
            complaint.Id,
            true,
            reportedUserName,
            request.BanReason);
        
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
            throw new UserFriendlyException("Complaint not found");
        }
        
        if (complaint.Status == ComplaintStatus.Resolved || complaint.Status == ComplaintStatus.Dismissed)
        {
            throw new UserFriendlyException("This complain was reviewed");
        }

        complaint.Status = request.Status;
        await _complaintRepository.UpdateAsync(complaint);

        var reportedUser = await _userRepository.GetByIdAsync(complaint.ReportedUserId);
        var reportedUserName = reportedUser.IdentityUser.UserName ?? "Unknown";

        var notification = await _userBanService.CreateComplaintResolutionNotificationAsync(
            complaint.ReporterId,
            complaint.Id,
            false,
            reportedUserName);
        
        var notificationDto = ObjectMapper.Map<Notifications.Notification, MelodyMatch.Notification.DTOs.Responses.NotificationResponseDto>(notification);
        await _notificationSenderService.SendNotificationToUserAsync(complaint.ReporterId, notificationDto);

        return ObjectMapper.Map<Complaints.Complaint, ComplaintResponseDto>(complaint);
    }
    
    [Authorize(Roles = RolesConsts.Admin)]
    public async Task UnbanUserAsync(UnbanUserRequestDto request)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new UserFriendlyException("User not found");
        }
        
        await _userBanService.UnbanUserAsync(request.UserId, request.Reason);
        
        var complaint = await _complaintRepository.GetByIdAsync(request.ComplaintId);
        complaint.Status = ComplaintStatus.Dismissed;
        await _complaintRepository.UpdateAsync(complaint, true);
    }
    
    [Authorize(Roles = RolesConsts.Admin)]
    public async Task<ComplaintResponseDto> UpdateComplaintStatusAsync(UpdateComplaintStatusRequestDto request)
    {
        var complaint = await _complaintRepository.GetByIdAsync(request.ComplaintId);
        
        if (complaint == null)
        {
            throw new UserFriendlyException("СComplaint not found");
        }
        
        complaint.Status = request.Status;
        await _complaintRepository.UpdateAsync(complaint);
        
        return ObjectMapper.Map<Complaints.Complaint, ComplaintResponseDto>(complaint);
    }
}