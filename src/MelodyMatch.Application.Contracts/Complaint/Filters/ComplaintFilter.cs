using System;
using MelodyMatch.Enums.Complaints;
using Volo.Abp.Application.Dtos;

namespace MelodyMatch.Complaint.Filters;

public class ComplaintFilter : PagedAndSortedResultRequestDto
{
    public Guid? ReporterId { get; set; } = default;
    
    public Guid? ReportedUserId { get; set; } = default;
    
    public string? Reason { get; set; } = default;
    
    public ComplaintStatus? Status { get; set; } = default;
    
    public DateTime? CreatedAtFrom { get; set; } = default;
    
    public DateTime? CreatedAtTo { get; set; } = default;
}