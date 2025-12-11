using System;
using System.Threading.Tasks;
using MelodyMatch.MelodyMatchUser.DTOs.Requests;
using MelodyMatch.MelodyMatchUser.DTOs.Responses;
using MelodyMatch.MelodyMatchUser.Filters;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MelodyMatch.MelodyMatchUser.Services;

public interface IMelodyMatchUserApplicationService : IApplicationService
{
    Task<MelodyMatchUserResponseDto> CreateAsync(CreateMelodyMatchUserRequestDto request);
    
    Task<MelodyMatchUserResponseDto> UpdateAsync(UpdateMelodyMatchUserRequestDto request);
    
    Task<PagedResultDto<MelodyMatchUserResponseDto>> GetListAsync(MelodyMatchUserFilter filter);
    
    Task<MelodyMatchUserResponseDto> GetByIdAsync(Guid id);

    Task<MelodyMatchUserResponseDto> GetByUsernameAsync(string username);
    
    Task<MelodyMatchUserResponseDto> GetByIdentityUserIdAsync(Guid identityUserId);
    
    Task<MelodyMatchUserResponseDto> GetWithProfileByIdentityUserIdAsync(Guid identityUserId);
    
    Task<MelodyMatchUserResponseDto> ClearAvatarAsync(Guid id);
    
    Task DeleteByIdAsync(Guid id);
    
    Task DeleteByIdentityUserIdAsync(Guid identityUserId);
}