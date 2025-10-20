using System;
using MelodyMatch.Chats;
using MelodyMatch.Enums.Reactions;
using MelodyMatch.Users;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Guids;

namespace MelodyMatch.Reactions;

public class Reaction : FullAuditedAggregateRoot<Guid>
{
    public Guid FromUserId { get; set; }
    
    public MelodyMatchUser FromUser { get; set; }
    
    public Guid ToUserId { get; set; }
    
    public MelodyMatchUser ToUser { get; set; }
    
    public ReactionType Type { get; set; }
    
    public string? Message { get; set; }

    public Reaction()
    {
    }

    public Reaction(Guid fromUserId, Guid toUserId, ReactionType type, string? message)
    {
        Id = SimpleGuidGenerator.Instance.Create();
        FromUserId = fromUserId;
        ToUserId = toUserId;
        Type = type;
        Message = message;
    }

    public Reaction(Guid id, Guid fromUserId, Guid toUserId, ReactionType type, string? message) : base(id)
    {
        FromUserId = fromUserId;
        ToUserId = toUserId;
        Type = type;
        Message = message;
    }
}