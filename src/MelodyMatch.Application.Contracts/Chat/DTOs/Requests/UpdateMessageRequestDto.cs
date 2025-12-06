using System;

namespace MelodyMatch.Chat.DTOs.Requests;

public class UpdateMessageRequestDto : CreateMessageRequestDto
{
    public Guid Id { get; set; }
    
    public bool IsRead { get; set; }
}