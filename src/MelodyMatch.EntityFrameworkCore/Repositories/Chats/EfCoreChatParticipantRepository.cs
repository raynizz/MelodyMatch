using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MelodyMatch.Chats;
using MelodyMatch.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MelodyMatch.Repositories.Chats;

public class EfCoreChatParticipantRepository : EfCoreRepository<MelodyMatchDbContext, ChatParticipant>, IChatParticipantRepository
{
    public EfCoreChatParticipantRepository(IDbContextProvider<MelodyMatchDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<List<ChatParticipant>> GetByUserIdAsync(Guid userId)
    {
        var dbContext = await GetDbContextAsync();
        
        return await dbContext.ChatParticipants
            .Include(x => x.Chat)
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task<bool> ExistsInChatAsync(Guid chatId, Guid userId)
    {
        var dbContext = await GetDbContextAsync();
        
        return await dbContext.ChatParticipants
            .AnyAsync(x => x.ChatId == chatId && x.UserId == userId);
    }

    public async Task<List<ChatParticipant>> GetByChatIdAsync(Guid chatId)
    {
        var dbContext = await GetDbContextAsync();
        
        return await dbContext.ChatParticipants
            .Where(x => x.ChatId == chatId)
            .ToListAsync();
    }
}
