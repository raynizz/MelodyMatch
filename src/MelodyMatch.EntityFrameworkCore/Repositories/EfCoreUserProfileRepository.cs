using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MelodyMatch.Constants;
using MelodyMatch.DTOs.UserProfile.DbRequests;
using MelodyMatch.EntityFrameworkCore;
using MelodyMatch.Enums.MelodyMatchUser;
using MelodyMatch.UserProfiles;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MelodyMatch.Repositories;

public class EfCoreUserProfileRepository : EfCoreRepository<MelodyMatchDbContext, UserProfile, Guid>, IUserProfileRepository
{
    public EfCoreUserProfileRepository(IDbContextProvider<MelodyMatchDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
    
    public async Task<UserProfile> AddUserProfileAsync(UserProfile userProfileToAdd)
    {
        var dbContext = await GetDbContextAsync();
        
        await dbContext.UserProfiles.AddAsync(userProfileToAdd);
        await dbContext.SaveChangesAsync();
        
        return userProfileToAdd;
    }
    
    public async Task<UserProfile> GetByIdAsync(Guid id)
    {
        var dbContext = await GetDbContextAsync();
        
        return await dbContext.UserProfiles
            .Include(x => x.MelodyMatchUser)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<UserProfile> GetByMelodyMatchUserIdAsync(Guid melodyMatchUserId)
    {
        var dbContext = await GetDbContextAsync();
        
        return await dbContext.UserProfiles
            .Include(x => x.MelodyMatchUser)
            .FirstOrDefaultAsync(x => x.MelodyMatchUserId == melodyMatchUserId);
    }

    public async Task DeleteByMelodyMatchUserIdAsync(Guid melodyMatchUserId)
    {
        var dbContext = await GetDbContextAsync();
        
        var userProfile = await dbContext.UserProfiles
            .FirstOrDefaultAsync(x => x.MelodyMatchUserId == melodyMatchUserId);

        if (userProfile != null)
        {
            dbContext.UserProfiles.Remove(userProfile);
            await dbContext.SaveChangesAsync();
        }
    }
    
    public async Task<List<UserProfile>> GetCandidatesForUserAsync(GetUserProfilesDbRequestDto request)
    {
        var dbContext = await GetDbContextAsync();

        var query = dbContext.UserProfiles
            .Include(x => x.MelodyMatchUser)
            .ThenInclude(x => x.IdentityUser)
            .AsQueryable();

        query = query.Where(p =>
            p.MelodyMatchUserId != request.CurrentMelodyMatchUserId &&
            !request.ExcludeUserIds.Contains(p.MelodyMatchUserId));

        query = query.Where(p =>
            request.PreferredGenders.Contains(p.MelodyMatchUser.Gender) &&
            p.PreferredGenders.Contains(request.CurrentUserGender));

        var userMinAge = request.MinAge ?? Math.Max(UserConsts.MinAge, request.CurrentUserAge - UserConsts.DefaultAgeGap);
        var userMaxAge = request.MaxAge ?? (request.CurrentUserAge + UserConsts.DefaultAgeGap);

        query = query.Where(p =>
            p.Age >= userMinAge &&
            p.Age <= userMaxAge &&

            request.CurrentUserAge >= (p.PreferredMinAge ?? (((p.Age - UserConsts.DefaultAgeGap) >= UserConsts.MinAge) ? (p.Age - UserConsts.DefaultAgeGap) : UserConsts.MinAge)) &&
            request.CurrentUserAge <= (p.PreferredMaxAge ?? (p.Age + UserConsts.DefaultAgeGap))
        );

        if (!string.IsNullOrWhiteSpace(request.Location))
        {
            query = query.Where(p =>
                p.Location == request.Location ||
                EF.Functions.Like(p.Location, $"%{request.Location}%"));
        }

        return await query.ToListAsync();
    }
}