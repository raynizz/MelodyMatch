using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MelodyMatch.Chats;

public interface IChatParticipantRepository : IRepository<ChatParticipant>
{
    Task<List<ChatParticipant>> GetByUserIdAsync(Guid userId);
    Task<bool> ExistsInChatAsync(Guid chatId, Guid userId);
}