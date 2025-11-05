using System;
using System.Linq;
using System.Threading.Tasks;
using MelodyMatch.EntityFrameworkCore;
using MelodyMatch.UserProfiles;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MelodyMatch.Repositories;

public class EfCoreUserProfileRepository : EfCoreRepository<MelodyMatchDbContext, UserProfile, Guid>, IUserProfileRepository
{
    public EfCoreUserProfileRepository(IDbContextProvider<MelodyMatchDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
    
    public async Task<UserProfile> AddUserProfileAsync(UserProfile userProfileToAdd)
    {
        var dbContext = await GetDbContextAsync();
        
        await dbContext.UserProfiles.AddAsync(userProfileToAdd);
        await dbContext.SaveChangesAsync();
        
        return userProfileToAdd;
    }
    
    public async Task<UserProfile> GetByIdAsync(Guid id)
    {
        var dbContext = await GetDbContextAsync();
        
        return await dbContext.UserProfiles
            .Include(x => x.MelodyMatchUser)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<UserProfile> GetByMelodyMatchUserIdAsync(Guid melodyMatchUserId)
    {
        var dbContext = await GetDbContextAsync();
        
        return await dbContext.UserProfiles
            .Include(x => x.MelodyMatchUser)
            .FirstOrDefaultAsync(x => x.MelodyMatchUserId == melodyMatchUserId);
    }

    public async Task DeleteByMelodyMatchUserIdAsync(Guid melodyMatchUserId)
    {
        var dbContext = await GetDbContextAsync();
        
        var userProfile = await dbContext.UserProfiles
            .FirstOrDefaultAsync(x => x.MelodyMatchUserId == melodyMatchUserId);

        if (userProfile != null)
        {
            dbContext.UserProfiles.Remove(userProfile);
            await dbContext.SaveChangesAsync();
        }
    }
}