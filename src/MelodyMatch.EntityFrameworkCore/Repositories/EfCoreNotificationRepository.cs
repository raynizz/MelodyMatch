using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MelodyMatch.EntityFrameworkCore;
using MelodyMatch.Notifications;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MelodyMatch.Repositories;

public class EfCoreNotificationRepository : EfCoreRepository<MelodyMatchDbContext, Notification, Guid>, INotificationRepository
{
    public EfCoreNotificationRepository(IDbContextProvider<MelodyMatchDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
    
    public async Task<List<Notification>> GetUserNotificationsAsync(Guid userId, bool? isRead = null)
    {
        var dbContext = await GetDbContextAsync();
        
        var query = dbContext.Notifications
            .Where(x => x.UserId == userId && !x.IsDeleted);
        
        if (isRead.HasValue)
        {
            query = query.Where(x => x.IsRead == isRead.Value);
        }
        
        return await query
            .OrderByDescending(x => x.CreationTime)
            .ToListAsync();
    }
    
    public async Task<int> GetUnreadCountAsync(Guid userId)
    {
        var dbContext = await GetDbContextAsync();
        
        return await dbContext.Notifications
            .Where(x => x.UserId == userId && !x.IsRead && !x.IsDeleted)
            .CountAsync();
    }
    
    public async Task MarkAsReadAsync(Guid notificationId)
    {
        var dbContext = await GetDbContextAsync();
        
        var notification = await dbContext.Notifications
            .FirstOrDefaultAsync(x => x.Id == notificationId);
        
        if (notification != null)
        {
            notification.IsRead = true;
            await dbContext.SaveChangesAsync();
        }
    }
    
    public async Task MarkAllAsReadAsync(Guid userId)
    {
        var dbContext = await GetDbContextAsync();
        
        await dbContext.Notifications
            .Where(x => x.UserId == userId && !x.IsRead)
            .ForEachAsync(x => x.IsRead = true);
        
        await dbContext.SaveChangesAsync();
    }
}

