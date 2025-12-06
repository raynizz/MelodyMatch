using System;
using MelodyMatch.Enums.MelodyMatchUser;

namespace MelodyMatch.MelodyMatchUser.DTOs.Requests;

public class CreateMelodyMatchUserRequestDto
{
    public Guid IdentityUserId { get; set; }
    
    public GenderType Gender { get; set; }
    
    public string AvatarUrl { get; set; } = string.Empty;
    
    public CreateMelodyMatchUserRequestDto()
    {
    }
}