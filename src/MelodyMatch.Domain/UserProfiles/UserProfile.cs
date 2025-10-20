using System;
using System.Collections.Generic;
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
    
    // TODO: add preferred genders, age range, distance radius, etc.
    
    public List<string> ProfilePhotoUrls { get; set; } // TODO: consider storing photos in a blob storage and saving the URL here

    public UserProfile()
    {
    }
    
    public UserProfile(Guid id, Guid melodyMatchUserId, int age, string bio, string location, List<string> profilePhotoUrls)
        : base(id)
    {
        MelodyMatchUserId = melodyMatchUserId;
        Age = age;
        Bio = bio;
        Location = location;
        ProfilePhotoUrls = profilePhotoUrls;
    }

    public UserProfile(Guid melodyMatchUserId, int age, string bio, string location, List<string> profilePhotoUrls)
    {
        Id = SimpleGuidGenerator.Instance.Create();
        MelodyMatchUserId = melodyMatchUserId;
        Age = age;
        Bio = bio;
        Location = location;
        ProfilePhotoUrls = profilePhotoUrls;
    }
    
    // TODO: add spotify integration properties (favorite genres, artists, etc.)
}