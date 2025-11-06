using System;

namespace MelodyMatch.MelodyMatchUser.DTOs.Requests;

public class UpdateMelodyMatchUserRequestDto : CreateMelodyMatchUserRequestDto
{
    public Guid Id { get; set; }
    
    public UpdateMelodyMatchUserRequestDto()
    {
    }
}