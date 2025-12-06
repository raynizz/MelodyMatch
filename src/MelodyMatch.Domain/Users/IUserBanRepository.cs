using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MelodyMatch.Users;

public interface IUserBanRepository : IRepository<UserBan, Guid>
{
    Task<UserBan?> GetActiveBanByUserIdAsync(Guid userId);
    
    Task<bool> IsUserBannedAsync(Guid userId);
}

