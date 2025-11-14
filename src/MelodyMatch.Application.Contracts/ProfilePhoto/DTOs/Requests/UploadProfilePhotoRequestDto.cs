using System;
using Microsoft.AspNetCore.Http;

namespace MelodyMatch.ProfilePhoto.DTOs.Requests;

public class UploadProfilePhotoRequestDto
{
    public Guid UserProfileId { get; set; }
    
    public IFormFile File { get; set; } = default!;
}