using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MelodyMatch.Chats;

public interface IMessageRepository : IRepository<Message, Guid>
{
    Task<List<Message>> GetMessagesByChatIdAsync(Guid chatId);
    
    Task<List<Message>> GetUnreadMessagesAsync(Guid userId);
}