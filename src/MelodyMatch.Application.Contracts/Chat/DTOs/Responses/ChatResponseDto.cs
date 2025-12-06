using System;
using System.Collections.Generic;
using MelodyMatch.MelodyMatchUser.DTOs.Responses;
using Volo.Abp.Application.Dtos;

namespace MelodyMatch.Chat.DTOs.Responses;

public class ChatResponseDto : FullAuditedEntityDto<Guid>
{
    public List<MelodyMatchUserResponseDto> Participants { get; set; } = new();
    
    public List<MessageResponseDto> Messages { get; set; } = new();
    
    public MessageResponseDto? LastMessage { get; set; }
    
    public int UnreadCount { get; set; }
}