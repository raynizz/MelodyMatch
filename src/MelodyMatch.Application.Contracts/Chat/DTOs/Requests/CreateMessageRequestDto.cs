using System;

namespace MelodyMatch.Chat.DTOs.Requests;

public class CreateMessageRequestDto
{
    public Guid ChatId { get; set; }
    
    public string Content { get; set; } = string.Empty;
}