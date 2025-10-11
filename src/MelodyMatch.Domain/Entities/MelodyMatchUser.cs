using System;
using MelodyMatch.Enums.MelodyMatchUser;
using Volo.Abp.Domain.Entities.Auditing;

namespace MelodyMatch.Entities;

public class MelodyMatchUser : FullAuditedAggregateRoot<Guid>
{
    public string LastName { get; set; } = default!;

    public string FirstName { get; set; } = default!;
    
    public string UserName { get; set; } = default!;
    
    public string Email { get; set; } = default!;
    
    public bool? IsActive { get; set; } = true;

    public GenderType Gender { get; set; } = GenderType.NotSpecified;

    // TODO: add roles, spotify, music preferences, etc.
}