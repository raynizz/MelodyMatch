using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MelodyMatch.EntityFrameworkCore;
using MelodyMatch.ShownUserProfiles;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MelodyMatch.Repositories;

public class EfCoreShownUserProfileRepository : EfCoreRepository<MelodyMatchDbContext, ShownUserProfile, Guid>, IShownUserProfileRepository
{
    public EfCoreShownUserProfileRepository(IDbContextProvider<MelodyMatchDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
    
    public async Task<List<Guid>> GetRecentlyShownUserIdsAsync(Guid userId, int days)
    {
        var dbContext = await GetDbContextAsync();
        return await dbContext.ShownUserProfiles
            .Where(x => x.UserId == userId && x.ShownAt > DateTime.UtcNow.AddDays(-days))
            .Select(x => x.ShownUserId)
            .ToListAsync();
    }
    
    public async Task AddShownUserAsync(Guid userId, Guid shownUserId)
    {
        var dbContext = await GetDbContextAsync();
        
        var shownUserProfile = new ShownUserProfile
        {
            UserId = userId,
            ShownUserId = shownUserId,
            ShownAt = DateTime.UtcNow
        };
        
        await dbContext.ShownUserProfiles.AddAsync(shownUserProfile);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> IsUserShown(Guid userId, Guid shownUserId)
    {
        var dbContext = await GetDbContextAsync();
        
        return await dbContext.ShownUserProfiles
            .AnyAsync(x => x.UserId == userId && x.ShownUserId == shownUserId);
    }

    public async Task<List<ShownUserProfile>> GetExpiredShownUserProfilesAsync(DateTime expirationDate)
    {
        var dbContext = await GetDbContextAsync();
        
        return await dbContext.ShownUserProfiles
            .Where(x => x.ShownAt < expirationDate && !x.Reacted)
            .ToListAsync();
    }
}