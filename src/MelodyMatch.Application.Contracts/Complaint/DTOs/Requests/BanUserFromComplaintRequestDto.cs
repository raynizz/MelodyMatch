using System;

namespace MelodyMatch.Complaint.DTOs.Requests;

public class BanUserFromComplaintRequestDto
{
    public Guid ComplaintId { get; set; }
    
    public string BanReason { get; set; } = default!;
    
    // All bans are permanent, no expiration date needed
}

