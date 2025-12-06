using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MelodyMatch.EntityFrameworkCore;
using MelodyMatch.Enums.Reactions;
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
    
    public async Task<List<Guid>> GetReactedUserIdsAsync(Guid fromUserId, ReactionType? reactionType = null)
    {
        var dbContext = await GetDbContextAsync();
        
        var reactions = dbContext.Reactions
            .Where(r => r.FromUserId == fromUserId);
        
        if (reactionType != null)
        {
            reactions = reactions.Where(r => r.Type == reactionType.Value);
        }
        
        return await reactions
            .Select(r => r.ToUserId)
            .Distinct()
            .ToListAsync();
    }

    public async Task<List<Reaction>> GetLikesForCurrentUserAsync(Guid toUserId)
    {
        var dbContext = await GetDbContextAsync();
        
        return await dbContext.Reactions
            .Include(x => x.FromUser)
                .ThenInclude(x => x.UserProfile)
                    .ThenInclude(x => x.ProfilePhotos)
            .Include(x => x.FromUser)
                .ThenInclude(x => x.IdentityUser)
            .Where(r => r.ToUserId == toUserId &&
                        (r.Type == ReactionType.Like || r.Type == ReactionType.LikeWithMessage))
            .ToListAsync();
    }

    public async Task<List<Guid>> GetLikerUserIdsForCurrentUserAsync(Guid toUserId)
    {
        var dbContext = await GetDbContextAsync();
        
        return await dbContext.Reactions
            .Where(r => r.ToUserId == toUserId &&
                        (r.Type == ReactionType.Like || r.Type == ReactionType.LikeWithMessage))
            .Select(r => r.FromUserId)
            .Distinct()
            .ToListAsync();
    }
}