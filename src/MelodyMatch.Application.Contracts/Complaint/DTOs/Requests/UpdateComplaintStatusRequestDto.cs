using System;
using MelodyMatch.Enums.Complaints;

namespace MelodyMatch.Complaint.DTOs.Requests;

public class UpdateComplaintStatusRequestDto
{
    public Guid ComplaintId { get; set; }
    
    public ComplaintStatus Status { get; set; }
    
    public string? Note { get; set; }
}
