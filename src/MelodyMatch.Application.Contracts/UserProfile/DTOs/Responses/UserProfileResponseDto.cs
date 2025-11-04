using System;
using System.Collections.Generic;
using MelodyMatch.Enums.MelodyMatchUser;
using MelodyMatch.Enums.UserProfile;
using Volo.Abp.Application.Dtos;

namespace MelodyMatch.UserProfile.DTOs.Responses;

public class UserProfileResponseDto : FullAuditedEntityDto<Guid>
{
    // public MelodyMatchUserResponseDto MelodyMatchUser { get; set;  }

    public int Age { get; set; } = default;

    public string Bio { get; set; } = default;
    
    public string Location { get; set; } = default;

    public List<GenderType> PreferredGenders { get; set; } = default;
    
    public int? PreferredMinAge { get; set; } = default;
    
    public int? PreferredMaxAge { get; set; } = default;

    public List<InterestType>? Interests { get; set; } = default;

    public List<string> ProfilePhotoUrls { get; set; } = default;
    
    public UserProfileResponseDto()
    {
    }
}