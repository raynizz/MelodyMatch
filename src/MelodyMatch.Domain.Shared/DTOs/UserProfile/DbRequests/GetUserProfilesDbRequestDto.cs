using System;
using System.Collections.Generic;
using MelodyMatch.Enums.MelodyMatchUser;

namespace MelodyMatch.DTOs.UserProfile.DbRequests;

public class GetUserProfilesDbRequestDto
{
    public Guid CurrentMelodyMatchUserId { get; set; }
    
    public string Location { get; set; } = string.Empty;
    
    public List<Guid> ExcludeUserIds { get; set; } = [];
    
    public GenderType CurrentUserGender { get; set; }
    
    public List<GenderType> PreferredGenders { get; set; } = [];
    
    public int CurrentUserAge { get; set; }
    
    public int? MinAge { get; set; }
    
    public int? MaxAge { get; set; }

    public GetUserProfilesDbRequestDto(
        Guid currentMelodyMatchUserId,
        string location,
        List<Guid> excludeUserIds,
        GenderType currentUserGender,
        List<GenderType> preferredGenders,
        int currentUserAge,
        int? minAge = null,
        int? maxAge = null)
    {
        CurrentMelodyMatchUserId = currentMelodyMatchUserId;
        Location = location;
        ExcludeUserIds = excludeUserIds;
        CurrentUserGender = currentUserGender;
        PreferredGenders = preferredGenders;
        CurrentUserAge = currentUserAge;
        MinAge = minAge;
        MaxAge = maxAge;
    }
}