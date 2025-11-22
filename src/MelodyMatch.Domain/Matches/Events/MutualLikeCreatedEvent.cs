using System;
using Volo.Abp.EventBus;

namespace MelodyMatch.Matches.Events;

[EventName("MelodyMatch.Matches.MutualLikeCreated")]
public class MutualLikeCreatedEvent
{
    public Guid UserAId { get; set; }
    
    public Guid UserBId { get; set; }
    
    public string? FirstMessage { get; set; }
    
    public Guid? FirstMessageSenderId { get; set; }

    public MutualLikeCreatedEvent()
    {
    }

    public MutualLikeCreatedEvent(
        Guid userAId,
        Guid userBId,
        string? firstMessage = null,
        Guid? firstMessageSenderId = null)
    {
        UserAId = userAId;
        UserBId = userBId;
        FirstMessage = firstMessage;
        FirstMessageSenderId = firstMessageSenderId;
    }
}