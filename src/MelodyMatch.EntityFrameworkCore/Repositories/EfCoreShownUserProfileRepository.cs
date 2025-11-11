using System;
using MelodyMatch.EntityFrameworkCore;
using MelodyMatch.ShownUserProfiles;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MelodyMatch.Repositories;

public class EfCoreShownUserProfileRepository : EfCoreRepository<MelodyMatchDbContext, ShownUserProfile, Guid>, IShownUserProfileRepository
{
    public EfCoreShownUserProfileRepository(IDbContextProvider<MelodyMatchDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
}