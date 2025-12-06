using System;
using MelodyMatch.MelodyMatchUser.DTOs.Responses;
using Volo.Abp.Application.Dtos;

namespace MelodyMatch.Chat.DTOs.Responses;

public class MessageResponseDto : FullAuditedEntityDto<Guid>
{
    public MelodyMatchUserResponseDto Sender { get; set; }
    
    public string Content { get; set; } = default!;
    
    public bool IsRead { get; set; }
    
    public Guid ChatId { get; set; }
}