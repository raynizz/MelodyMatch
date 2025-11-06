using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MelodyMatch.Extensions;
using MelodyMatch.MelodyMatchUser.DTOs.Requests;
using MelodyMatch.MelodyMatchUser.DTOs.Responses;
using MelodyMatch.MelodyMatchUser.Filters;
using MelodyMatch.MelodyMatchUser.Services;
using MelodyMatch.Users;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;

namespace MelodyMatch.MelodyMatchUser;

public class MelodyMatchUserApplicationService : ApplicationService, IMelodyMatchUserApplicationService
{
    private readonly IMelodyMatchUserRepository _melodyMatchUserRepository;
    
    public MelodyMatchUserApplicationService(
        IMelodyMatchUserRepository melodyMatchUserRepository)
    {
        _melodyMatchUserRepository = melodyMatchUserRepository;
    }
    
    public async Task<MelodyMatchUserResponseDto> CreateAsync(CreateMelodyMatchUserRequestDto request)
    {
        var melodyMatchUser = ObjectMapper.Map<CreateMelodyMatchUserRequestDto, Users.MelodyMatchUser>(request);
        var createdMelodyMatchUser = await _melodyMatchUserRepository.AddMelodyMatchUserAsync(melodyMatchUser);
        
        return ObjectMapper.Map<Users.MelodyMatchUser, MelodyMatchUserResponseDto>(createdMelodyMatchUser);
    }

    public async Task<MelodyMatchUserResponseDto> UpdateAsync(UpdateMelodyMatchUserRequestDto request)
    {
        var melodyMatchUser = await _melodyMatchUserRepository.GetByIdAsync(request.Id);
        
        ObjectMapper.Map(request, melodyMatchUser);
        
        var updatedMelodyMatchUser = await _melodyMatchUserRepository.UpdateAsync(melodyMatchUser);
        
        return ObjectMapper.Map<Users.MelodyMatchUser, MelodyMatchUserResponseDto>(updatedMelodyMatchUser);
    }

    public async Task<PagedResultDto<MelodyMatchUserResponseDto>> GetListAsync(MelodyMatchUserFilter filter)
    {
        var query = await _melodyMatchUserRepository.GetQueryableAsync();

        query = query
            .FilterBy(filter.Gender != null, x => x.Gender == filter.Gender);
        
        // TODO: add more filters as needed
        
        var totalCount = await AsyncExecuter.CountAsync(query);
        
        query = query
            .OrderByDescending(x => x.CreationTime)
            .Skip(filter.SkipCount)
            .Take(filter.MaxResultCount);
        
        var melodyMatchUsers = await AsyncExecuter.
            ToListAsync(
                query
                    .Include(x => x.IdentityUser)
                    .Include(x => x.UserProfile));
        
        return new PagedResultDto<MelodyMatchUserResponseDto>(
            totalCount,
            ObjectMapper.Map<List<Users.MelodyMatchUser>, List<MelodyMatchUserResponseDto>>(melodyMatchUsers));
    }

    public async Task<MelodyMatchUserResponseDto> GetByIdAsync(Guid id)
    {
        var melodyMatchUser = await _melodyMatchUserRepository.GetByIdAsync(id);
        
        return ObjectMapper.Map<Users.MelodyMatchUser, MelodyMatchUserResponseDto>(melodyMatchUser);
    }

    public async Task<MelodyMatchUserResponseDto> GetByIdentityUserIdAsync(Guid identityUserId)
    {
        var melodyMatchUser = await _melodyMatchUserRepository.GetByIdentityUserIdAsync(identityUserId);
        
        return ObjectMapper.Map<Users.MelodyMatchUser, MelodyMatchUserResponseDto>(melodyMatchUser);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        await _melodyMatchUserRepository.DeleteByIdAsync(id);
    }

    public async Task DeleteByIdentityUserIdAsync(Guid identityUserId)
    {
        await _melodyMatchUserRepository.DeleteByIdentityUserIdAsync(identityUserId);
    }
}