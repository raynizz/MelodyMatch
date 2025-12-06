using System;
using System.Linq;
using System.Threading.Tasks;
using MelodyMatch.EntityFrameworkCore;
using MelodyMatch.Users;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MelodyMatch.Repositories;

public class EfCoreUserBanRepository : EfCoreRepository<MelodyMatchDbContext, UserBan, Guid>, IUserBanRepository
{
    public EfCoreUserBanRepository(IDbContextProvider<MelodyMatchDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
    
    public async Task<UserBan?> GetActiveBanByUserIdAsync(Guid userId)
    {
        var dbContext = await GetDbContextAsync();
        var now = DateTime.UtcNow;
        
        return await dbContext.UserBans
            .Where(x => x.UserId == userId && 
                       x.IsActive && 
                       !x.IsDeleted &&
                       (x.IsPermanent || x.ExpiresAt == null || x.ExpiresAt > now))
            .OrderByDescending(x => x.CreationTime)
            .FirstOrDefaultAsync();
    }
    
    public async Task<bool> IsUserBannedAsync(Guid userId)
    {
        var ban = await GetActiveBanByUserIdAsync(userId);
        return ban != null;
    }
}

