using System;

namespace MelodyMatch.UserProfile.DTOs.Requests;

public class UpdateUserProfileRequestDto : CreateUserProfileRequestDto
{
    public Guid Id { get; set; }
}