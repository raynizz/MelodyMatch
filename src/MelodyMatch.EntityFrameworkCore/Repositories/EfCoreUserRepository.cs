using System;
using System.Threading.Tasks;
using MelodyMatch.Entities;
using MelodyMatch.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MelodyMatch.Repositories;

public class EfCoreUserRepository : EfCoreRepository<MelodyMatchDbContext, MelodyMatchUser, Guid>, IUserRepository
{
    public EfCoreUserRepository(
        IDbContextProvider<MelodyMatchDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<MelodyMatchUser?> GetUserByEmail(string email)
    {
        var dbContext = await GetDbContextAsync();

        return await dbContext
            .MelodyMatchUsers
            .FirstOrDefaultAsync(x => x.Email == email);
    }
}