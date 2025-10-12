using System;
using MelodyMatch.Enums.MelodyMatchUser;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Guids;
using Volo.Abp.Identity;

namespace MelodyMatch.Users;

public class MelodyMatchUser : FullAuditedAggregateRoot<Guid>
{
    public Guid IdentityUserId { get; set; }
    
    public IdentityUser IdentityUser { get; set; }

    public GenderType Gender { get; set; } = GenderType.NotSpecified;

    // TODO: add roles, spotify, music preferences, etc.

    protected MelodyMatchUser()
    {
    }

    public MelodyMatchUser(Guid identityUserId, GenderType gender)
    {
        Id = SimpleGuidGenerator.Instance.Create();
        IdentityUserId = identityUserId;
        Gender = gender;
    }

    public MelodyMatchUser(Guid id, Guid identityUserId, GenderType gender) : base(id)
    {
        IdentityUserId = identityUserId;
        Gender = gender;
    }
}