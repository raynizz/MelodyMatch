using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MelodyMatch.UserProfile.DTOs.Requests;
using MelodyMatch.UserProfile.DTOs.Responses;
using MelodyMatch.UserProfile.Filters;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MelodyMatch.UserProfile.Services;

public interface IUserProfileApplicationService : IApplicationService
{
    Task<UserProfileResponseDto> GetByMelodyMatchUserIdAsync(Guid melodyMatchUserId);

    Task<UserProfileResponseDto> GetByIdAsync(Guid id);
    
    Task<UserProfileResponseDto> CreateAsync(CreateUserProfileRequestDto request);
    
    Task<UserProfileResponseDto> UpdateAsync(UpdateUserProfileRequestDto request);
    
    Task<UserProfileResponseDto> ClearBioAsync(Guid id);
    
    Task DeleteByMelodyMatchUserIdAsync(Guid melodyMatchUserId);
    
    Task<PagedResultDto<UserProfileResponseDto>> GetListAsync(UserProfileFilter filter);
}