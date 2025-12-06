using System;
using System.Threading.Tasks;
using MelodyMatch.Reaction.DTOs.Requests;
using MelodyMatch.Reaction.DTOs.Responses;
using MelodyMatch.Reaction.Filters;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MelodyMatch.Reaction.Services;

public interface IReactionApplicationService : IApplicationService
{
    Task<PagedResultDto<ReactionResponseDto>> GetListAsync(ReactionFilter filter);
    
    Task<ReactionResponseDto> AddReactionAsync(CreateReactionRequestDto request);
    
    Task<ReactionResponseDto> UpdateReactionAsync(UpdateReactionRequestDto request);
    
    Task DeleteReactionAsync(Guid id);
}