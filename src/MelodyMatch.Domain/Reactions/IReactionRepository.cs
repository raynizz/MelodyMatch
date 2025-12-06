using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MelodyMatch.Enums.Reactions;
using Volo.Abp.Domain.Repositories;

namespace MelodyMatch.Reactions;

public interface IReactionRepository : IRepository<Reaction, Guid>
{
    Task<Reaction> AddReactionAsync(Reaction reactionToAdd);
    
    Task<Reaction> GetByIdAsync(Guid id);
    
    Task<List<Guid>> GetReactedUserIdsAsync(Guid fromUserId, ReactionType? reactionType = null);
    
    Task<List<Reaction>> GetLikesForCurrentUserAsync(Guid toUserId);
    
    Task<List<Guid>> GetLikerUserIdsForCurrentUserAsync(Guid toUserId);
}