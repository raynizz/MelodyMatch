using System;
using MelodyMatch.Enums.Reactions;
using MelodyMatch.MelodyMatchUser.DTOs.Responses;
using Volo.Abp.Application.Dtos;

namespace MelodyMatch.Reaction.DTOs.Responses;

public class ReactionResponseDto : FullAuditedEntityDto<Guid>
{
    public MelodyMatchUserResponseDto FromUser { get; set; }
    
    public MelodyMatchUserResponseDto ToUser { get; set; }
    
    public ReactionType Type { get; set; }
    
    public string? Message { get; set; }
}