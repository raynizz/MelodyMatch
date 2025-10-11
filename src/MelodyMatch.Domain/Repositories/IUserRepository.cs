using System;
using System.Threading.Tasks;
using MelodyMatch.Entities;
using Volo.Abp.Domain.Repositories;

namespace MelodyMatch.Repositories;

public interface IUserRepository : IRepository<MelodyMatchUser, Guid>
{
    Task<MelodyMatchUser?> GetUserByEmail(string email);
}