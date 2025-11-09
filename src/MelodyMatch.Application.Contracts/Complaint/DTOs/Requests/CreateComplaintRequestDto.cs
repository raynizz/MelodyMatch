using System;
using MelodyMatch.Enums.Complaints;

namespace MelodyMatch.Complaint.DTOs.Requests;

public class CreateComplaintRequestDto
{
    public Guid ReporterId { get; set; }
    
    public Guid ReportedUserId { get; set; }
    
    public string Reason { get; set; } //TODO: make enum
    
    public ComplaintStatus Status { get; set; } = ComplaintStatus.Sent;
    
    public CreateComplaintRequestDto()
    {
    }
}