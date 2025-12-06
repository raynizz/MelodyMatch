using System;

namespace MelodyMatch.Complaint.DTOs.Requests;

public class UnbanUserRequestDto
{
    public string? Reason { get; set; }

    public Guid UserId { get; set; }
    
    public Guid ComplaintId { get; set; }
}
