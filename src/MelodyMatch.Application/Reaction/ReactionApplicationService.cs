using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MelodyMatch.Extensions;
using MelodyMatch.Reaction.DTOs.Requests;
using MelodyMatch.Reaction.DTOs.Responses;
using MelodyMatch.Reaction.Filters;
using MelodyMatch.Reaction.Services;
using MelodyMatch.Reactions;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MelodyMatch.Reaction;

public class ReactionApplicationService : ApplicationService, IReactionApplicationService
{
    private readonly IReactionRepository _reactionRepository;

    public ReactionApplicationService(
        IReactionRepository reactionRepository)
    {
        _reactionRepository = reactionRepository;
    }
    
    public async Task<PagedResultDto<ReactionResponseDto>> GetListAsync(ReactionFilter filter)
    {
        var query = await _reactionRepository.GetQueryableAsync();
        
        query = query
            .FilterBy(filter.FromUserId != null, x => x.FromUserId == filter.FromUserId)
            .FilterBy(filter.ToUserId != null, x => x.ToUserId == filter.ToUserId)
            .FilterBy(filter.Type != null, x => x.Type == filter.Type)
            .FilterBy(!string.IsNullOrEmpty(filter.Message), x => x.Message.Contains(filter.Message!));
        
        var totalCount = await AsyncExecuter.CountAsync(query);
        
        query = query
            .OrderByDescending(x => x.CreationTime)
            .Skip(filter.SkipCount)
            .Take(filter.MaxResultCount);

        var reactions = await AsyncExecuter.ToListAsync(
            query
                .Include(x => x.FromUser)
                .ThenInclude(x => x.UserProfile)
                .Include(x => x.ToUser)
                .ThenInclude(x => x.UserProfile));

        return new PagedResultDto<ReactionResponseDto>(
            totalCount,
            ObjectMapper.Map<List<Reactions.Reaction>, List<ReactionResponseDto>>(reactions));
    }

    public async Task<ReactionResponseDto> AddReactionAsync(CreateReactionRequestDto request)
    {
        var reaction = ObjectMapper.Map<CreateReactionRequestDto, Reactions.Reaction>(request);
        var createdReaction = await _reactionRepository.AddReactionAsync(reaction);
        
        return ObjectMapper.Map<Reactions.Reaction, ReactionResponseDto>(createdReaction);
    }

    public async Task<ReactionResponseDto> UpdateReactionAsync(UpdateReactionRequestDto request)
    {
        var reaction = await _reactionRepository.GetByIdAsync(request.Id);
        
        ObjectMapper.Map(request, reaction);
        
        var updatedReaction = await _reactionRepository.UpdateAsync(reaction);
        
        return ObjectMapper.Map<Reactions.Reaction, ReactionResponseDto>(updatedReaction);
    }

    public async Task DeleteReactionAsync(Guid id)
    {
        var reaction = await _reactionRepository.GetByIdAsync(id);
        await _reactionRepository.DeleteAsync(reaction);
    }
}