using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MelodyMatch.Constants;
using MelodyMatch.Enums.Reactions;
using MelodyMatch.Extensions;
using MelodyMatch.Matches.Events;
using MelodyMatch.Reaction.DTOs.Requests;
using MelodyMatch.Reaction.DTOs.Responses;
using MelodyMatch.Reaction.Filters;
using MelodyMatch.Reaction.Services;
using MelodyMatch.Reactions;
using MelodyMatch.ShownUserProfiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Local;

namespace MelodyMatch.Reaction;

[Authorize(Roles = RolesConsts.Dater)]
public class ReactionApplicationService : ApplicationService, IReactionApplicationService
{
    private readonly IReactionRepository _reactionRepository;
    private readonly IShownUserProfileRepository _shownUserRepository;
    private readonly ILocalEventBus _localEventBus;

    public ReactionApplicationService(
        IReactionRepository reactionRepository,
        IShownUserProfileRepository shownUserRepository,
        ILocalEventBus localEventBus)
    {
        _reactionRepository = reactionRepository;
        _shownUserRepository = shownUserRepository;
        _localEventBus = localEventBus;
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
        
        if (!await _shownUserRepository.IsUserShown(request.FromUserId, request.ToUserId))
        {
            await _shownUserRepository.AddShownUserAsync(request.FromUserId, request.ToUserId);
        }
        
        if(reaction.Type is ReactionType.Like or ReactionType.LikeWithMessage)
        {
            var oppositeLike = await _reactionRepository.FindAsync(r => 
                r.FromUserId == request.ToUserId && 
                r.ToUserId == request.FromUserId && 
                (r.Type == ReactionType.Like || r.Type == ReactionType.LikeWithMessage));
            
            if (oppositeLike != null)
            {
                string? firstMessage;
                Guid? firstMessageSenderId;

                if (oppositeLike.Type == ReactionType.LikeWithMessage)
                {
                    firstMessage = oppositeLike.Message;
                    firstMessageSenderId = request.ToUserId;
                }
                else
                {
                    firstMessage = request.Message;
                    firstMessageSenderId = request.FromUserId;
                }
                
                var mutualLikeCreatedEvent = new MutualLikeCreatedEvent(
                    userAId: reaction.FromUserId,
                    userBId: reaction.ToUserId,
                    firstMessage: firstMessage,
                    firstMessageSenderId: firstMessageSenderId
                );

                await _localEventBus.PublishAsync(mutualLikeCreatedEvent);
            }
        }
        
        return ObjectMapper.Map<Reactions.Reaction, ReactionResponseDto>(createdReaction);
    }

    public async Task<ReactionResponseDto> UpdateReactionAsync(UpdateReactionRequestDto request)
    {
        var reaction = await _reactionRepository.GetByIdAsync(request.Id);
        
        ObjectMapper.Map(request, reaction);
        
        var updatedReaction = await _reactionRepository.UpdateAsync(reaction);

        if (!await _shownUserRepository.IsUserShown(request.FromUserId, request.ToUserId))
        {
            await _shownUserRepository.AddShownUserAsync(request.FromUserId, request.ToUserId);
        }
        
        return ObjectMapper.Map<Reactions.Reaction, ReactionResponseDto>(updatedReaction);
    }

    public async Task DeleteReactionAsync(Guid id)
    {
        var reaction = await _reactionRepository.GetByIdAsync(id);
        await _reactionRepository.DeleteAsync(reaction);
    }
}