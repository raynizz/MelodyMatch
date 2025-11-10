using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MelodyMatch.Chats;

public interface IChatRepository : IRepository<Chat, Guid>
{
    Task<Chat?> GetWithParticipantsAsync(Guid chatId);
    
    Task<List<Chat>> GetUserChatsAsync(Guid userId);
}