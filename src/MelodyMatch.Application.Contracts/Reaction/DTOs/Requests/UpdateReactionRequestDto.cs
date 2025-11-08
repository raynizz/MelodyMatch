using System;

namespace MelodyMatch.Reaction.DTOs.Requests;

public class UpdateReactionRequestDto : CreateReactionRequestDto
{
    public Guid Id { get; set; }
}