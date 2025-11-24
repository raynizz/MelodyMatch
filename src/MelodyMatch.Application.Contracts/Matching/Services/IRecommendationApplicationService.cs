using System.Collections.Generic;
using System.Threading.Tasks;
using MelodyMatch.Matching.DTOs.Responses;
using Volo.Abp.Application.Services;

namespace MelodyMatch.Matching.Services;

public interface IRecommendationApplicationService : IApplicationService
{
    Task<List<SuggestedUserResponseDto>> GetSuggestionsForCurrentUserAsync(int? take = 10);

    Task<List<SuggestedUserResponseDto>> GetLikeCurrentProfileUsers(int? take = 10);
}