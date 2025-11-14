using System;
using System.Collections.Generic;
using MelodyMatch.Enums.MelodyMatchUser;
using MelodyMatch.Enums.UserProfile;
using MelodyMatch.ProfilePhotos;
using MelodyMatch.Users;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Guids;

namespace MelodyMatch.UserProfiles;

public class UserProfile : FullAuditedAggregateRoot<Guid>
{
    public Guid MelodyMatchUserId { get; set; }
    
    public MelodyMatchUser MelodyMatchUser { get; set; }
    
    public int Age { get; set; }
    
    public string Bio { get; set; }
    
    public string Location { get; set; }
    
    public List<GenderType> PreferredGenders { get; set; }
    
    public int? PreferredMinAge { get; set; }
    
    public int? PreferredMaxAge { get; set; }
    
    public List<InterestType>? Interests { get; set; }
    
    public List<ProfilePhoto>? ProfilePhotos { get; set; }
    
    // TODO: add distance radius, etc.
    
    public UserProfile()
    {
    }
    
    public UserProfile(
        Guid id,
        Guid melodyMatchUserId,
        int age,
        string bio,
        string location,
        List<GenderType> preferredGenders,
        int? preferredMinAge = null,
        int? preferredMaxAge = null,
        List<InterestType>? interests = null,
        List<ProfilePhoto>? profilePhotos = null)
        : base(id)
    {
        MelodyMatchUserId = melodyMatchUserId;
        Age = age;
        Bio = bio;
        Location = location;
        PreferredGenders = preferredGenders;
        PreferredMinAge = preferredMinAge;
        PreferredMaxAge = preferredMaxAge;
        Interests = interests;
        ProfilePhotos = profilePhotos;
    }

    public UserProfile(
        Guid melodyMatchUserId,
        int age,
        string bio,
        string location,
        List<string> profilePhotoUrls,
        List<GenderType> preferredGenders,
        int? preferredMinAge = null,
        int? preferredMaxAge = null,
        List<InterestType>? interests = null,
        List<ProfilePhoto>? profilePhotos = null)
    {
        Id = SimpleGuidGenerator.Instance.Create();
        MelodyMatchUserId = melodyMatchUserId;
        Age = age;
        Bio = bio;
        Location = location;
        PreferredGenders = preferredGenders;
        PreferredMinAge = preferredMinAge;
        PreferredMaxAge = preferredMaxAge;
        Interests = interests;
        ProfilePhotos = profilePhotos;
    }
    
    // TODO: add spotify integration properties (favorite genres, artists, etc.)
}