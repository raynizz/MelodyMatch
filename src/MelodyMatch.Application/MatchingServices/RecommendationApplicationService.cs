using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MelodyMatch.Constants;
using MelodyMatch.Contexts.MelodyMatchUser;
using MelodyMatch.DTOs.UserProfile.DbRequests;
using MelodyMatch.Enums.UserProfile;
using MelodyMatch.Enums.Reactions;
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
    private readonly IUserBanRepository _userBanRepository;
    
    public RecommendationApplicationService(
        IUserProfileRepository userProfileRepository,
        IReactionRepository reactionRepository,
        IShownUserProfileRepository shownUserRepository,
        ICurrentMelodyMatchUser currentMelodyMatchUser,
        IStringLocalizer<MelodyMatchResource> localizer,
        IUserBanRepository userBanRepository)
    {
        _userProfileRepository = userProfileRepository;
        _reactionRepository = reactionRepository;
        _shownUserRepository = shownUserRepository;
        _currentMelodyMatchUser = currentMelodyMatchUser;
        _localizer = localizer;
        _userBanRepository = userBanRepository;
    }

    public async Task<List<SuggestedUserResponseDto>> GetSuggestionsForCurrentUserAsync(int? take = 10)
    {
        var currentUserId = await _currentMelodyMatchUser.GetIdAsync();

        var currentProfile = await _userProfileRepository.GetByMelodyMatchUserIdAsync(currentUserId);
        if (currentProfile == null)
        {
            throw new NotFoundException(MelodyMatchDomainErrorCodes.UserProfile.UserProfileNotFilled);
        }

        var reportedIds = await _reactionRepository.GetReactedUserIdsAsync(currentUserId, ReactionType.Report);
        var oppositeLikedIds = await _reactionRepository.GetLikerUserIdsForCurrentUserAsync(currentUserId);
        var shownIds = await _shownUserRepository.GetRecentlyShownUserIdsAsync(currentUserId, 3);

        var now = DateTime.UtcNow;
        var bannedUserIds = await GetActiveBannedUserIdsAsync(now);

        var excludedIds = reportedIds
            .Concat(shownIds)
            .Concat(oppositeLikedIds)
            .Concat(bannedUserIds)
            .Distinct()
            .ToList();

        var candidates = await _userProfileRepository.GetCandidatesForUserAsync(new GetUserProfilesDbRequestDto(
            currentUserId,
            currentProfile.Location,
            excludedIds,
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

    public async Task<List<SuggestedUserResponseDto>> GetLikeCurrentProfileUsers(int? take = 10)
    {
        var currentUserId = await _currentMelodyMatchUser.GetIdAsync();

        var currentProfile = await _userProfileRepository.GetByMelodyMatchUserIdAsync(currentUserId);
        if (currentProfile == null)
        {
            throw new NotFoundException(MelodyMatchDomainErrorCodes.UserProfile.UserProfileNotFilled);
        }
        
        var likesForCurrentUser = await _reactionRepository.GetLikesForCurrentUserAsync(currentUserId);
        var shownIds = await _shownUserRepository.GetRecentlyShownUserIdsAsync(currentUserId, 3);
        var candidatesLikes = likesForCurrentUser
            .Where(p => !shownIds.Contains(p.FromUserId))
            .ToList();
        
        return candidatesLikes.Select(p => new SuggestedUserResponseDto
        {
            MelodyMatchUserId = p.FromUserId,
            Name = p.FromUser.IdentityUser.Name,
            Age = p.FromUser.UserProfile.Age,
            Location = p.FromUser.UserProfile.Location,
            Bio = p.FromUser.UserProfile.Bio,
            PhotosUrls = p.FromUser.UserProfile.ProfilePhotos?.Select(x => x.Url)?.ToList(),
            Interests = p.FromUser.UserProfile.Interests?.Select(i => i.GetLocalizedDescription(_localizer)).ToList(),
            Gender = p.FromUser.Gender.GetLocalizedDescription(_localizer),
            Message = p.Type == ReactionType.LikeWithMessage ? p.Message : string.Empty
        })
            .Take(take ?? 10)
            .ToList();
    }

    private static double ComputeCompatibilityScore(UserProfiles.UserProfile current, UserProfiles.UserProfile other)
    {
        var currentVector = BuildInterestVector(current);
        var otherVector = BuildInterestVector(other);

        var interestSimilarity = CosineSimilarity(currentVector, otherVector);

        var ageFactor = 1 - (Math.Abs(current.Age - other.Age) / 50.0);
        ageFactor = Math.Max(0, ageFactor);

        var score = interestSimilarity * 0.7 + ageFactor * 0.3;

        return score;
    }

    private static List<int> BuildInterestVector(UserProfiles.UserProfile profile)
    {
        var interestsCount = Enum.GetValues<InterestType>().Length;
        var vector = new int[interestsCount];

        if (profile.Interests != null)
        {
            foreach (var interest in profile.Interests)
            {
                vector[(int)interest] = 1;
            }
        }

        return vector.ToList();
    }

    private static double CosineSimilarity(List<int> vectorA, List<int> vectorB)
    {
        double dot = 0;
        double magA = 0;
        double magB = 0;

        for (var i = 0; i < vectorA.Count; i++)
        {
            dot += vectorA[i] * vectorB[i];
            magA += vectorA[i] * vectorA[i];
            magB += vectorB[i] * vectorB[i];
        }

        if (magA == 0 || magB == 0)
        {
            return 0;
        }

        return dot / (Math.Sqrt(magA) * Math.Sqrt(magB));
    }
    
    private async Task<List<Guid>> GetActiveBannedUserIdsAsync(DateTime now)
    {
        var query = await _userBanRepository.GetQueryableAsync();

        return await AsyncExecuter.ToListAsync(
            query
                .Where(x => x.IsActive &&
                            !x.IsDeleted &&
                            (x.IsPermanent || x.ExpiresAt == null || x.ExpiresAt > now))
                .Select(x => x.UserId)
        );
    }
}
