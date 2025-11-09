using System;
using MelodyMatch.Enums.Complaints;
using MelodyMatch.MelodyMatchUser.DTOs.Responses;
using Volo.Abp.Application.Dtos;

namespace MelodyMatch.Complaint.DTOs.Responses;

public class ComplaintResponseDto : FullAuditedEntityDto<Guid>
{
    public MelodyMatchUserResponseDto Reporter { get; set; }
    
    public MelodyMatchUserResponseDto ReportedUser { get; set; }
    
    public string Reason { get; set; } // TODO: make enum
    
    public ComplaintStatus Status { get; set; }
}