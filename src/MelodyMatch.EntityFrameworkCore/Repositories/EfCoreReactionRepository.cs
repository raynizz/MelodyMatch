using System;
using System.Threading.Tasks;
using MelodyMatch.EntityFrameworkCore;
using MelodyMatch.Reactions;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MelodyMatch.Repositories;

public class EfCoreReactionRepository : EfCoreRepository<MelodyMatchDbContext, Reaction, Guid>, IReactionRepository
{
    public EfCoreReactionRepository(IDbContextProvider<MelodyMatchDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
    
    public async Task<Reaction> AddReactionAsync(Reaction reactionToAdd)
    {
        var dbContext = await GetDbContextAsync();
        
        await dbContext.Reactions.AddAsync(reactionToAdd);
        await dbContext.SaveChangesAsync();
        
        return reactionToAdd;
    }
    
    public async Task<Reaction> GetByIdAsync(Guid id)
    {
        var dbContext = await GetDbContextAsync();
        
        return await dbContext.Reactions
            .Include(x => x.FromUser)
                .ThenInclude(x => x.UserProfile)
            .Include(x => x.ToUser)
                .ThenInclude(x => x.UserProfile)
            .FirstOrDefaultAsync(x => x.Id == id);
        
    }
}