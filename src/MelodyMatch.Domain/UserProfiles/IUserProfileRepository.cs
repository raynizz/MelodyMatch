using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MelodyMatch.DTOs.UserProfile.DbRequests;
using Volo.Abp.Domain.Repositories;

namespace MelodyMatch.UserProfiles;

public interface IUserProfileRepository : IRepository<UserProfile, Guid>
{
    Task<UserProfile> AddUserProfileAsync(UserProfile userProfileToAdd);
    
    Task<UserProfile> GetByIdAsync(Guid id);
    
    Task<UserProfile> GetByMelodyMatchUserIdAsync(Guid melodyMatchUserId);
    
    Task DeleteByMelodyMatchUserIdAsync(Guid melodyMatchUserId);

    Task<List<UserProfile>> GetCandidatesForUserAsync(GetUserProfilesDbRequestDto request);

}