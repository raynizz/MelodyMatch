using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MelodyMatch.Complaint.DTOs.Requests;
using MelodyMatch.Complaint.DTOs.Responses;
using MelodyMatch.Complaint.Filters;
using MelodyMatch.Complaint.Services;
using MelodyMatch.Complaints;
using MelodyMatch.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MelodyMatch.Complaint;

[Authorize]
public class ComplaintApplicationService : ApplicationService, IComplaintApplicationService
{
    private readonly IComplaintRepository _complaintRepository;
    
    public ComplaintApplicationService(
        IComplaintRepository complaintRepository)
    {
        _complaintRepository = complaintRepository;
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

    public async Task DeleteComplaintAsync(Guid id)
    {
        var complaint = await _complaintRepository.GetByIdAsync(id);
        await _complaintRepository.DeleteAsync(complaint);
    }
}