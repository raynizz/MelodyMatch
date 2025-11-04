using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MelodyMatch.Extensions;
using MelodyMatch.UserProfile.DTOs.Requests;
using MelodyMatch.UserProfile.DTOs.Responses;
using MelodyMatch.UserProfile.Filters;
using MelodyMatch.UserProfile.Services;
using MelodyMatch.UserProfiles;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Microsoft.EntityFrameworkCore;

namespace MelodyMatch.UserProfile;

public class UserProfileApplicationService : ApplicationService, IUserProfileApplicationService
{
    private readonly IUserProfileRepository _userProfileRepository;
    
    public UserProfileApplicationService(
        IUserProfileRepository userProfileRepository)
    {
        _userProfileRepository = userProfileRepository;
    }
    
    public async Task<UserProfileResponseDto> GetUserProfileByUserIdAsync(Guid melodyMatchUserId)
    {
        var userProfile = await _userProfileRepository.GetByMelodyMatchUserId(melodyMatchUserId);
        
        return ObjectMapper.Map<UserProfiles.UserProfile, UserProfileResponseDto>(userProfile);
    }
    
    public async Task<UserProfileResponseDto> GetUserProfileByIdAsync(Guid id)
    {
        var userProfile = await _userProfileRepository.GetByIdAsync(id);
        
        return ObjectMapper.Map<UserProfiles.UserProfile, UserProfileResponseDto>(userProfile);
    }

    public async Task<UserProfileResponseDto> CreateAsync(CreateUserProfileRequestDto request)
    {
        var userProfile = ObjectMapper.Map<CreateUserProfileRequestDto, UserProfiles.UserProfile>(request);
        var createdUserProfile = await _userProfileRepository.AddUserProfileAsync(userProfile);
        
        return ObjectMapper.Map<UserProfiles.UserProfile, UserProfileResponseDto>(createdUserProfile);
    }

    public async Task<UserProfileResponseDto> UpdateUserProfileAsync(UpdateUserProfileRequestDto request)
    {
        var userProfile = await _userProfileRepository.GetByIdAsync(request.Id);
        
        ObjectMapper.Map(request, userProfile);
        
        var updatedUserProfile = await _userProfileRepository.UpdateAsync(userProfile);
        
        return ObjectMapper.Map<UserProfiles.UserProfile, UserProfileResponseDto>(updatedUserProfile);
    }

    public async Task DeleteByMelodyMatchUserIdAsync(Guid melodyMatchUserId)
    {
        await _userProfileRepository.DeleteByMelodyMatchUserIdAsync(melodyMatchUserId);
    }

    public async Task<PagedResultDto<UserProfileResponseDto>> GetListAsync(UserProfileFilter filter)
    {
        var query = await _userProfileRepository.GetQueryableAsync();
        
        query = query
            .FilterBy(filter.Age != null, x => x.Age == filter.Age)
            .FilterBy(!string.IsNullOrEmpty(filter.Location), x => x.Location.Contains(filter.Location!))
            .FilterBy(filter.PreferredGenders is { Count: > 0 }, x => x.PreferredGenders.Any(gender => filter.PreferredGenders!.Contains(gender)))
            .FilterBy(filter.PreferredMinAge != null, x => x.PreferredMinAge >= filter.PreferredMinAge)
            .FilterBy(filter.PreferredMaxAge != null, x => x.PreferredMaxAge <= filter.PreferredMaxAge);
        
        var totalCount = await AsyncExecuter.CountAsync(query);

        query = query
            .OrderByDescending(x => x.CreationTime)
            .Skip(filter.SkipCount)
            .Take(filter.MaxResultCount);
        
        var userProfiles = await AsyncExecuter
            .ToListAsync(
                query.Include(x => x.MelodyMatchUser));

        return new PagedResultDto<UserProfileResponseDto>(
            totalCount,
            ObjectMapper.Map<List<UserProfiles.UserProfile>, List<UserProfileResponseDto>>(userProfiles));
    }
}