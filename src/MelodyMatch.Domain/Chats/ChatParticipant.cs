using System;
using MelodyMatch.Users;
using Volo.Abp.Domain.Entities.Auditing;

namespace MelodyMatch.Chats;

public class ChatParticipant : FullAuditedEntity
{
    public Guid ChatId { get; set; }
    
    public Chat Chat { get; set; } = default!;

    public Guid UserId { get; set; }
    
    public MelodyMatchUser User { get; set; } = default!;

    public override object[] GetKeys() => new object[] { ChatId, UserId };
}