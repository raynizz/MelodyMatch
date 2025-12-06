using System;
using MelodyMatch.Enums.Complaints;
using MelodyMatch.MelodyMatchUser.DTOs.Responses;
using MelodyMatch.UserProfile.DTOs.Responses;
using Volo.Abp.Application.Dtos;

namespace MelodyMatch.Complaint.DTOs.Responses;

public class ComplaintResponseDto : FullAuditedEntityDto<Guid>
{
    public MelodyMatchUserResponseDto Reporter { get; set; } = default!;
    
    public MelodyMatchUserResponseDto ReportedUser { get; set; } = default!;
    
    public UserProfileResponseDto? ReportedUserProfile { get; set; }
    
    public string Reason { get; set; } = default!;
    
    public ComplaintStatus Status { get; set; }
}