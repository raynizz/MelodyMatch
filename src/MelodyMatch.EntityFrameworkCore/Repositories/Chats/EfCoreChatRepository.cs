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

public class EfCoreChatRepository : EfCoreRepository<MelodyMatchDbContext, Chat, Guid>, IChatRepository
{
    public EfCoreChatRepository(IDbContextProvider<MelodyMatchDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<Chat?> GetWithParticipantsAsync(Guid chatId)
    {
        var dbContext = await GetDbContextAsync();

        return await dbContext.Chats
            .Include(c => c.Participants)
            .ThenInclude(p => p.User)
            .ThenInclude(p => p.IdentityUser)
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.Id == chatId);
    }

    public async Task<List<Chat>> GetUserChatsAsync(Guid userId)
    {
        var dbContext = await GetDbContextAsync();

        return await dbContext.Chats
            .Include(c => c.Participants)
            .ThenInclude(p => p.User)
            .ThenInclude(p => p.IdentityUser)
            .Where(c => c.Participants.Any(p => p.UserId == userId))
            .ToListAsync();
    }
}