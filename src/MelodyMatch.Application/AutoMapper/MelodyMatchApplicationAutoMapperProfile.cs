using AutoMapper;
using MelodyMatch.MelodyMatchUser.DTOs.Requests;
using MelodyMatch.MelodyMatchUser.DTOs.Responses;
using MelodyMatch.UserProfile.DTOs.Requests;
using MelodyMatch.UserProfile.DTOs.Responses;
using Volo.Abp.AutoMapper;

namespace MelodyMatch.AutoMapper;

public class MelodyMatchApplicationAutoMapperProfile : Profile
{
    public MelodyMatchApplicationAutoMapperProfile()
    {
        MapUserProfile();
        MapMelodyMatchUser();
    }
    
    private void MapUserProfile()
    {
        CreateMap<CreateUserProfileRequestDto, UserProfiles.UserProfile>()
            .IgnoreAuditedObjectProperties();

        CreateMap<UpdateUserProfileRequestDto, UserProfiles.UserProfile>()
            .IgnoreAuditedObjectProperties();
        
        CreateMap<UserProfiles.UserProfile, UserProfileResponseDto>();
    }

    private void MapMelodyMatchUser()
    {
        CreateMap<CreateMelodyMatchUserRequestDto, Users.MelodyMatchUser>()
            .IgnoreAuditedObjectProperties();

        CreateMap<UpdateMelodyMatchUserRequestDto, Users.MelodyMatchUser>()
            .IgnoreAuditedObjectProperties();

        CreateMap<Users.MelodyMatchUser, MelodyMatchUserResponseDto>();
    }
}
