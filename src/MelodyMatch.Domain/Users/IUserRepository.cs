using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MelodyMatch.Users;

public interface IUserRepository : IRepository<MelodyMatchUser, Guid>
{
    Task<MelodyMatchUser?> GetUserByEmail(string email);
}