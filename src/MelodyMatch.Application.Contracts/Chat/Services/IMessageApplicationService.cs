using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MelodyMatch.Chat.DTOs.Requests;
using MelodyMatch.Chat.DTOs.Responses;
using Volo.Abp.Application.Services;

namespace MelodyMatch.Chat.Services;

public interface IMessageApplicationService : IApplicationService
{
    Task<List<MessageResponseDto>> GetMessagesAsync(Guid chatId);
    
    Task<MessageResponseDto> SendMessageAsync(CreateMessageRequestDto request);
    
    Task<List<MessageResponseDto>> GetUnreadMessagesAsync();
    
    Task<MessageResponseDto> UpdateMessageAsync(UpdateMessageRequestDto request);
}