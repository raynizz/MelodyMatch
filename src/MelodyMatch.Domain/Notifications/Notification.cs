using System;
using MelodyMatch.Enums.Notifications;
using MelodyMatch.Users;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Guids;

namespace MelodyMatch.Notifications;

public class Notification : FullAuditedAggregateRoot<Guid>
{
    public Guid UserId { get; set; }
    
    public MelodyMatchUser User { get; set; }
    
    public NotificationType Type { get; set; }
    
    public string Title { get; set; }
    
    public string Message { get; set; }
    
    public bool IsRead { get; set; }
    
    public Guid? RelatedEntityId { get; set; }

    protected Notification()
    {
    }
    
    public Notification(Guid userId, NotificationType type, string title, string message, Guid? relatedEntityId = null)
    {
        Id = SimpleGuidGenerator.Instance.Create();
        UserId = userId;
        Type = type;
        Title = title;
        Message = message;
        RelatedEntityId = relatedEntityId;
        IsRead = false;
    }
}
