using System;
using MelodyMatch.UserProfiles;
using Volo.Abp.Domain.Entities.Auditing;

namespace MelodyMatch.ProfilePhotos;

public class ProfilePhoto : FullAuditedAggregateRoot<Guid>
{
    public Guid UserProfileId { get; set; }
    
    public UserProfile UserProfile { get; set; }

    public string FileName { get; set; } = default!;

    public string Url { get; set; } = default!;

    public bool IsConfirmed { get; set; } = false;
}