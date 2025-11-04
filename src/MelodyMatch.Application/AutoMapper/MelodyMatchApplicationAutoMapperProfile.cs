using AutoMapper;
using MelodyMatch.UserProfile.DTOs.Requests;
using MelodyMatch.UserProfile.DTOs.Responses;
using Volo.Abp.AutoMapper;

namespace MelodyMatch.AutoMapper;

public class MelodyMatchApplicationAutoMapperProfile : Profile
{
    public MelodyMatchApplicationAutoMapperProfile()
    {
        MapUserProfile();
    }
    
    private void MapUserProfile()
    {
        CreateMap<CreateUserProfileRequestDto, UserProfiles.UserProfile>()
            .IgnoreAuditedObjectProperties();

        CreateMap<UpdateUserProfileRequestDto, UserProfiles.UserProfile>()
            .IgnoreAuditedObjectProperties();
        
        CreateMap<UserProfiles.UserProfile, UserProfileResponseDto>();
    }
}
