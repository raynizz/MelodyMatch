using System;
using MelodyMatch.Enums.Reactions;

namespace MelodyMatch.Reaction.DTOs.Requests;

public class CreateReactionRequestDto
{
    public Guid FromUserId { get; set; }
    
    public Guid ToUserId { get; set; }
    
    public ReactionType Type { get; set; }
    
    public string? Message { get; set; }
    
    public CreateReactionRequestDto()
    {
    }
}