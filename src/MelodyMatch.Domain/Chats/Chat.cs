using System;
using System.Collections.Generic;
using System.Linq;
using MelodyMatch.Users;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Guids;

namespace MelodyMatch.Chats;

public class Chat : FullAuditedAggregateRoot<Guid>
{
    public List<Message> Messages { get; set; } = new();
    public List<ChatParticipant> Participants { get; set; } = new();

    public Chat() { }

    public Chat(List<Guid> userIds)
    {
        Id = SimpleGuidGenerator.Instance.Create();
        Participants = userIds.ConvertAll(id => new ChatParticipant { ChatId = Id, UserId = id });
    }
    
    public Chat(Guid id, Guid userAId, Guid userBId) : base(id)
    {
        Messages = new List<Message>();
    }
    
    public bool HasParticipants(Guid userAId, Guid userBId)
    {
        var userIds = Participants.Select(p => p.UserId).ToList();
        return userIds.Contains(userAId) && userIds.Contains(userBId);
    }

    public Message AddMessage(Guid senderId, string content, bool isRead = false)
    {
        var message = new Message(
            senderId: senderId,
            content: content,
            isRead: isRead,
            chatId: Id
        );

        Messages.Add(message);
        return message;
    }
}