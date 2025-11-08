using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MelodyMatch.Reactions;

public interface IReactionRepository : IRepository<Reaction, Guid>
{
    Task<Reaction> AddReactionAsync(Reaction reactionToAdd);
    
    Task<Reaction> GetByIdAsync(Guid id);
}