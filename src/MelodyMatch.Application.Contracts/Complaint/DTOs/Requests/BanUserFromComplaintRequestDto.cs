using System;

namespace MelodyMatch.Complaint.DTOs.Requests;

public class BanUserFromComplaintRequestDto
{
    public Guid ComplaintId { get; set; }
    
    public string BanReason { get; set; }
    
    public DateTime? ExpiresAt { get; set; }
    
    public bool IsPermanent { get; set; } = false;
}

