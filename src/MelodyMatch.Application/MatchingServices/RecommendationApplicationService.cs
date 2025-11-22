using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MelodyMatch.Constants;
using MelodyMatch.Contexts.MelodyMatchUser;
using MelodyMatch.DTOs.UserProfile.DbRequests;
using MelodyMatch.Exceptions;
using MelodyMatch.Extensions;
using MelodyMatch.Localization;
using MelodyMatch.Matching.DTOs.Responses;
using MelodyMatch.Matching.Services;
using MelodyMatch.Reactions;
using MelodyMatch.ShownUserProfiles;
using MelodyMatch.UserProfiles;
using MelodyMatch.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Volo.Abp.Application.Services;

namespace MelodyMatch.MatchingServices;

[Authorize(Roles = RolesConsts.Dater)]
public class RecommendationApplicationService : ApplicationService, IRecommendationApplicationService
{
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly IReactionRepository _reactionRepository;
    private readonly IShownUserProfileRepository _shownUserRepository;
    private readonly ICurrentMelodyMatchUser _currentMelodyMatchUser;
    private readonly IStringLocalizer<MelodyMatchResource> _localizer;
    
    public RecommendationApplicationService(
        IUserProfileRepository userProfileRepository,
        IReactionRepository reactionRepository,
        IShownUserProfileRepository shownUserRepository,
        ICurrentMelodyMatchUser currentMelodyMatchUser,
        IStringLocalizer<MelodyMatchResource> localizer)
    {
        _userProfileRepository = userProfileRepository;
        _reactionRepository = reactionRepository;
        _shownUserRepository = shownUserRepository;
        _currentMelodyMatchUser = currentMelodyMatchUser;
        _localizer = localizer;
    }


    public async Task<List<SuggestedUserResponseDto>> GetSuggestionsForCurrentUserAsync(int? take = 10)
    {
        var currentUserId = await _currentMelodyMatchUser.GetIdAsync();

        var currentProfile = await _userProfileRepository.GetByMelodyMatchUserIdAsync(currentUserId);
        if (currentProfile == null)
        {
            throw new NotFoundException(MelodyMatchDomainErrorCodes.UserProfile.UserProfileNotFilled);
        }

        // var reactedIds = await _reactionRepository.GetReactedUserIdsAsync(currentUserId);
        var shownIds = await _shownUserRepository.GetRecentlyShownUserIdsAsync(currentUserId, 3);

        // var excludedIds = reactedIds.Concat(shownIds).Distinct().ToList();

        var candidates = await _userProfileRepository.GetCandidatesForUserAsync(new GetUserProfilesDbRequestDto(
            currentUserId,
            currentProfile.Location,
            shownIds,
            currentProfile.MelodyMatchUser.Gender,
            currentProfile.PreferredGenders,
            currentProfile.Age,
            currentProfile.PreferredMinAge,
            currentProfile.PreferredMaxAge));

        var scored = candidates
            .Select(p => new
            {
                Profile = p,
                Score = ComputeCompatibilityScore(currentProfile, p)
            })
            .OrderByDescending(x => x.Score)
            .Take(take ?? 10)
            .Select(x => x.Profile)
            .ToList();

        return scored.Select(p => new SuggestedUserResponseDto
        {
            MelodyMatchUserId = p.MelodyMatchUserId,
            Name = p.MelodyMatchUser.IdentityUser.Name,
            Age = p.Age,
            Location = p.Location,
            Bio = p.Bio,
            PhotosUrls = p.ProfilePhotos?.Select(x => x.Url)?.ToList(),
            Interests = p.Interests?.Select(i => i.GetLocalizedDescription(_localizer)).ToList(),
            Gender = p.MelodyMatchUser.Gender.GetLocalizedDescription(_localizer)
        }).ToList();
    }

    private double ComputeCompatibilityScore(UserProfiles.UserProfile current, UserProfiles.UserProfile other)
    {
        double score = 0;

        score += 1 - (Math.Abs(current.Age - other.Age) / 50.0);

        if (current.Interests != null && other.Interests != null)
        {
            var common = current.Interests.Intersect(other.Interests).Count();
            score += common * 0.3;
        }

        score *= Random.Shared.NextDouble() * 0.3 + 0.7;
        return score;
    }
}