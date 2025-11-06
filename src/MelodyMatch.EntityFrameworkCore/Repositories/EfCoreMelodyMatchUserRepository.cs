using System;
using System.Threading.Tasks;
using MelodyMatch.EntityFrameworkCore;
using MelodyMatch.Users;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MelodyMatch.Repositories;

public class EfCoreMelodyMatchUserRepository : EfCoreRepository<MelodyMatchDbContext, MelodyMatchUser, Guid>, IMelodyMatchUserRepository
{
    public EfCoreMelodyMatchUserRepository(
        IDbContextProvider<MelodyMatchDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<MelodyMatchUser?> GetUserByEmail(string email)
    {
        var dbContext = await GetDbContextAsync();

        return await dbContext
            .MelodyMatchUsers
            .Include(x => x.IdentityUser)
            .Include(x => x.UserProfile)
            .FirstOrDefaultAsync(x => x.IdentityUser.Email == email);
    }
    
    public async Task<MelodyMatchUser> AddMelodyMatchUserAsync(MelodyMatchUser melodyMatchUserToAdd)
    {
        var dbContext = await GetDbContextAsync();
        
        await dbContext.MelodyMatchUsers.AddAsync(melodyMatchUserToAdd);
        await dbContext.SaveChangesAsync();
        
        return melodyMatchUserToAdd;
    }
    
    public async Task<MelodyMatchUser> GetByIdentityUserIdAsync(Guid identityUserId)
    {
        var dbContext = await GetDbContextAsync();
        
        return await dbContext
            .MelodyMatchUsers
            .Include(x => x.IdentityUser)
            .Include(x => x.UserProfile)
            .FirstOrDefaultAsync(x => x.IdentityUserId == identityUserId);
    }
    
    public async Task DeleteByIdentityUserIdAsync(Guid identityUserId)
    {
        var dbContext = await GetDbContextAsync();
        
        var melodyMatchUser = await dbContext.MelodyMatchUsers
            .FirstOrDefaultAsync(x => x.IdentityUserId == identityUserId);

        if (melodyMatchUser != null)
        {
            dbContext.MelodyMatchUsers.Remove(melodyMatchUser);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<MelodyMatchUser> GetByIdAsync(Guid userId)
    {
        var dbContext = await GetDbContextAsync();
        
        return await dbContext
            .MelodyMatchUsers
            .Include(x => x.IdentityUser)
            .Include(x => x.UserProfile)
            .FirstOrDefaultAsync(x => x.Id == userId);
    }
    
    public async Task DeleteByIdAsync(Guid userId)
    {
        var dbContext = await GetDbContextAsync();
        
        var melodyMatchUser = await dbContext.MelodyMatchUsers
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (melodyMatchUser != null)
        {
            dbContext.MelodyMatchUsers.Remove(melodyMatchUser);
            await dbContext.SaveChangesAsync();
        }
    }
}