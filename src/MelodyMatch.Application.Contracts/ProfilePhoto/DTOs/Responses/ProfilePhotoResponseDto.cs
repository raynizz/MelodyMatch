using System;
using Volo.Abp.Application.Dtos;

namespace MelodyMatch.ProfilePhoto.DTOs.Responses;

public class ProfilePhotoResponseDto : FullAuditedEntityDto<Guid>
{
    public Guid UserProfileId { get; set; }
    
    public string Url { get; set; } = default!;
    
    public bool IsConfirmed { get; set; }
}