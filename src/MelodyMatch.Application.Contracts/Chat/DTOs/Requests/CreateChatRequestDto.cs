using System;
using System.Collections.Generic;

namespace MelodyMatch.Chat.DTOs.Requests;

public class CreateChatRequestDto
{
    public List<Guid> UserIds { get; set; } = new();
}