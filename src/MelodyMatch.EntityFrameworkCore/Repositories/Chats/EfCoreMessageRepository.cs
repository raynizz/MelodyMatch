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

public class EfCoreMessageRepository : EfCoreRepository<MelodyMatchDbContext, Message, Guid>, IMessageRepository
{
    public EfCoreMessageRepository(IDbContextProvider<MelodyMatchDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<List<Message>> GetMessagesByChatIdAsync(Guid chatId)
    {
        var dbContext = await GetDbContextAsync();
        
        return await dbContext.Messages
            .Where(m => m.ChatId == chatId)
            .OrderBy(m => m.CreationTime)
            .Include(m => m.Sender)
            .ThenInclude(u => u.IdentityUser)
            .ToListAsync();
    }

    public async Task<List<Message>> GetUnreadMessagesAsync(Guid userId)
    {
        var dbContext = await GetDbContextAsync();
        
        return await dbContext.Messages
            .Include(m => m.Chat)
            .ThenInclude(c => c.Participants)
            .ThenInclude(p => p.User)
            .ThenInclude(u => u.IdentityUser)
            .Where(m => m.SenderId != userId)
            .Where(m => !m.IsRead && m.Chat.Participants.Any(p => p.UserId == userId))
            .ToListAsync();
    }

    public async Task MarkMessagesAsReadAsync(List<Guid> messageIds, Guid userId)
    {
        var dbContext = await GetDbContextAsync();
        
        var messages = await dbContext.Messages
            .Where(m => messageIds.Contains(m.Id) && m.SenderId != userId)
            .ToListAsync();
        
        foreach (var message in messages)
        {
            message.IsRead = true;
        }
        
        await dbContext.SaveChangesAsync();
    }
}