using System;
using System.Threading.Tasks;
using MelodyMatch.Complaint.DTOs.Requests;
using MelodyMatch.Complaint.DTOs.Responses;
using MelodyMatch.Complaint.Filters;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MelodyMatch.Complaint.Services;

public interface IComplaintApplicationService : IApplicationService
{
    Task<PagedResultDto<ComplaintResponseDto>> GetListAsync(ComplaintFilter filter);
    
    Task<ComplaintResponseDto> GetByIdAsync(Guid id);
    
    Task<ComplaintResponseDto> AddComplaintAsync(CreateComplaintRequestDto request);
    
    Task<ComplaintResponseDto> UpdateComplaintAsync(UpdateComplaintRequestDto request);
    
    Task DeleteComplaintAsync(Guid id);
    
    Task<ComplaintResponseDto> BanUserFromComplaintAsync(BanUserFromComplaintRequestDto request);
    
    Task<ComplaintResponseDto> ResolveComplaintAsync(ResolveComplaintRequestDto request);
}