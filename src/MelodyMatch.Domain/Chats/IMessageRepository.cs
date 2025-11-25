using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MelodyMatch.Chats;

public interface IMessageRepository : IRepository<Message, Guid>
{
    Task<Message?> GetByIdWithSenderAsync(Guid id);
    
    Task<List<Message>> GetMessagesByChatIdAsync(Guid chatId);
    
    Task<List<Message>> GetUnreadMessagesAsync(Guid userId);
    
    Task MarkMessagesAsReadAsync(List<Guid> messageIds, Guid userId);
}