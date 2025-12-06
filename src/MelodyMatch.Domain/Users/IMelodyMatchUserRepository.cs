using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MelodyMatch.Users;

public interface IMelodyMatchUserRepository : IRepository<MelodyMatchUser, Guid>
{
    Task<MelodyMatchUser?> GetUserByEmail(string email);
    
    Task<MelodyMatchUser> AddMelodyMatchUserAsync(MelodyMatchUser melodyMatchUserToAdd);
    
    Task<MelodyMatchUser> GetByIdAsync(Guid userId);
    
    Task<MelodyMatchUser> GetByUsernameAsync(string username);
    
    Task<MelodyMatchUser> GetByIdentityUserIdAsync(Guid identityUserId);
    
    Task DeleteByIdAsync(Guid userId);
    
    Task DeleteByIdentityUserIdAsync(Guid identityUserId);
    
    Task<HashSet<string>> GetAllAvatarUrlsHashAsync();
}