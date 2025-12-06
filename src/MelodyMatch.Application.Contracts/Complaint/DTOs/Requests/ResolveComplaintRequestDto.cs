using System;
using MelodyMatch.Enums.Complaints;

namespace MelodyMatch.Complaint.DTOs.Requests;

public class ResolveComplaintRequestDto
{
    public Guid ComplaintId { get; set; }
    
    public ComplaintStatus Status { get; set; }
    
    public string? ResolutionNote { get; set; }
}

