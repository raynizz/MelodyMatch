using System;

namespace MelodyMatch.Complaint.DTOs.Requests;

public class UpdateComplaintRequestDto : CreateComplaintRequestDto
{
    public Guid Id { get; set; }
}