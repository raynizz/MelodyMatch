using System;
using System.Threading.Tasks;
using MelodyMatch.Chat.DTOs.Requests;
using MelodyMatch.Chat.DTOs.Responses;
using Volo.Abp.Application.Dtos;

namespace MelodyMatch.Chat.Services;

public interface IChatApplicationService
{
    Task<PagedResultDto<ChatResponseDto>> GetUserChatsAsync(PagedResultRequestDto input);
    
    Task<ChatResponseDto> GetChatAsync(Guid chatId);
    
    Task<ChatResponseDto> CreateChatAsync(CreateChatRequestDto request);
}