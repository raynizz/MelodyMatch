using System;
using MelodyMatch.Users;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Guids;

namespace MelodyMatch.Chats;

public class Message : FullAuditedAggregateRoot<Guid>
{
    
    // TODO: add statuses (e.g. replied, forwarded etc.)
    public Guid SenderId { get; set; }
    
    public MelodyMatchUser Sender { get; set; }
    
    public string Content { get; set; }

    public bool IsRead { get; set; } = false;
    
    public Guid ChatId { get; set; }
    
    public Chat Chat { get; set; }
    
    public Message()
    {
    }
    
    public Message(Guid senderId, string content, bool isRead, Guid chatId)
    {
        Id = SimpleGuidGenerator.Instance.Create();
        SenderId = senderId;
        Content = content;
        IsRead = isRead;
        ChatId = chatId;
    }
    
    public Message(Guid id, Guid senderId, string content, bool isRead, Guid chatId) : base(id)
    {
        SenderId = senderId;
        Content = content;
        IsRead = isRead;
        ChatId = chatId;
    }
}