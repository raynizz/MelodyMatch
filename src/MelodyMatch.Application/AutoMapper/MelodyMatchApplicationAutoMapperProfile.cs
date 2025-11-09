using AutoMapper;
using MelodyMatch.Complaint.DTOs.Requests;
using MelodyMatch.Complaint.DTOs.Responses;
using MelodyMatch.MelodyMatchUser.DTOs.Requests;
using MelodyMatch.MelodyMatchUser.DTOs.Responses;
using MelodyMatch.Reaction.DTOs.Requests;
using MelodyMatch.Reaction.DTOs.Responses;
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
        MapReactions();
        MapComplaints();
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
    
    private void MapReactions()
    {
        CreateMap<CreateReactionRequestDto, Reactions.Reaction>()
            .IgnoreAuditedObjectProperties();

        CreateMap<UpdateMelodyMatchUserRequestDto, Reactions.Reaction>()
            .IgnoreAuditedObjectProperties();
        
        CreateMap<Reactions.Reaction, ReactionResponseDto>();
    }

    private void MapComplaints()
    {
        CreateMap<CreateComplaintRequestDto, Complaints.Complaint>()
            .IgnoreAuditedObjectProperties();
        
        CreateMap<UpdateComplaintRequestDto, Complaints.Complaint>()
            .IgnoreAuditedObjectProperties();
        
        CreateMap<Complaints.Complaint, ComplaintResponseDto>();
    }
}
