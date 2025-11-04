using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MelodyMatch.UserProfiles;

public interface IUserProfileRepository : IRepository<UserProfile, Guid>
{
    Task<UserProfile> AddUserProfileAsync(UserProfile userProfileToAdd);
    
    Task<UserProfile> GetByIdAsync(Guid id);
    
    Task<UserProfile> GetByMelodyMatchUserId(Guid melodyMatchUserId);
    
    Task DeleteByMelodyMatchUserIdAsync(Guid melodyMatchUserId);
}