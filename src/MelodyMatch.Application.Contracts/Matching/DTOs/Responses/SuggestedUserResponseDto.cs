using System;
using System.Collections.Generic;
using MelodyMatch.Enums.MelodyMatchUser;

namespace MelodyMatch.Matching.DTOs.Responses;

public class SuggestedUserResponseDto
{
    public Guid MelodyMatchUserId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public int Age { get; set; }
    
    public string Location { get; set; } = string.Empty;
    
    public string Bio { get; set; } = string.Empty;

    public List<string> PhotosUrls { get; set; } = new();
    
    public List<string> Interests { get; set; } = new();
    
    public string Gender { get; set; } = string.Empty;
    
    public string Message { get; set; } = string.Empty;
}