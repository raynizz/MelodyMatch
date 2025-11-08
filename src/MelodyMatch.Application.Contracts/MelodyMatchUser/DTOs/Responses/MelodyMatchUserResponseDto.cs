using System;
using MelodyMatch.Enums.MelodyMatchUser;
using MelodyMatch.UserProfile.DTOs.Responses;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace MelodyMatch.MelodyMatchUser.DTOs.Responses;

public class MelodyMatchUserResponseDto : FullAuditedEntityDto<Guid>
{
    public IdentityUserDto IdentityUser { get; set; }
    
    public GenderType Gender { get; set; }
    
    public UserProfileResponseDto UserProfile { get; set; }
    
    public string AvatarUrl { get; set; } = string.Empty;
    
    //TODO: add another navigation properties if needed

    public MelodyMatchUserResponseDto()
    {
    }
}