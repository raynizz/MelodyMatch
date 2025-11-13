using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MelodyMatch.ShownUserProfiles;

public interface IShownUserProfileRepository : IRepository<ShownUserProfile, Guid>
{
    Task<List<Guid>> GetRecentlyShownUserIdsAsync(Guid userId, int days);
    
    Task AddShownUserAsync(Guid userId, Guid shownUserId);
    
    Task<bool> IsUserShown(Guid userId, Guid shownUserId);

    Task<List<ShownUserProfile>> GetExpiredShownUserProfilesAsync(DateTime expirationDate);
}