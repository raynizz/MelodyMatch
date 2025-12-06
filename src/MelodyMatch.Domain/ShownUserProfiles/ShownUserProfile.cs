using System;
using MelodyMatch.Users;
using Volo.Abp.Domain.Entities.Auditing;

namespace MelodyMatch.ShownUserProfiles;

public class ShownUserProfile : FullAuditedEntity<Guid>
{
    public Guid UserId { get; set; }
    
    public MelodyMatchUser User { get; set; }
    
    public Guid ShownUserId { get; set; }
    
    public MelodyMatchUser ShownUser { get; set; }
    
    public DateTime ShownAt { get; set; } = DateTime.UtcNow;
    
    public bool Reacted { get; set; } = false;

    public override object[] GetKeys() => new object[] { UserId, ShownUserId };

}