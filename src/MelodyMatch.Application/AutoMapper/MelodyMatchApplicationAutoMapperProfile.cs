using System.Linq;
using AutoMapper;
using MelodyMatch.Chat.DTOs.Requests;
using MelodyMatch.Chat.DTOs.Responses;
using MelodyMatch.Complaint.DTOs.Requests;
using MelodyMatch.Complaint.DTOs.Responses;
using MelodyMatch.MelodyMatchUser.DTOs.Requests;
using MelodyMatch.MelodyMatchUser.DTOs.Responses;
using MelodyMatch.ProfilePhoto.DTOs.Responses;
using MelodyMatch.Reaction.DTOs.Requests;
using MelodyMatch.Reaction.DTOs.Responses;
using MelodyMatch.UserProfile.DTOs.Requests;
using MelodyMatch.UserProfile.DTOs.Responses;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;

namespace MelodyMatch.AutoMapper;

public class MelodyMatchApplicationAutoMapperProfile : Profile
{
    public MelodyMatchApplicationAutoMapperProfile()
    {
        MapUserProfile();
        MapMelodyMatchUser();
        MapReactions();
        MapComplaints();
        MapChats();
        MapMessages();
        MapProfilePhotos();
        MapIdentityUser();
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
    
    private void MapChats()
    {
        CreateMap<Chats.Chat, ChatResponseDto>()
            .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants.Select(p => p.User)));
        
        CreateMap<CreateChatRequestDto, Chats.Chat>()
            .IgnoreAuditedObjectProperties();
    }
    
    private void MapMessages()
    {
        CreateMap<Chats.Message, MessageResponseDto>();
        
        CreateMap<CreateMessageRequestDto, Chats.Message>()
            .IgnoreAuditedObjectProperties();

        CreateMap<UpdateMessageRequestDto, Chats.Message>()
            .IgnoreAuditedObjectProperties();
    }
    
    private void MapProfilePhotos()
    {
        CreateMap<ProfilePhotos.ProfilePhoto, ProfilePhotoResponseDto>();
    }

    private void MapIdentityUser()
    {
        CreateMap<IdentityUserCreateDto, Volo.Abp.Identity.IdentityUser>()
            .IgnoreAuditedObjectProperties();

        CreateMap<IdentityUserUpdateDto, Volo.Abp.Identity.IdentityUser>()
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
            .IgnoreAuditedObjectProperties();
    }
}
