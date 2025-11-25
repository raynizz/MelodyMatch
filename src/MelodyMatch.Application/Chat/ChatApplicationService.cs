using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MelodyMatch.Chat.DTOs.Requests;
using MelodyMatch.Chat.DTOs.Responses;
using MelodyMatch.Chat.Services;
using MelodyMatch.Chats;
using MelodyMatch.Constants;
using MelodyMatch.Contexts.MelodyMatchUser;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace MelodyMatch.Chat;

[Authorize(Roles = RolesConsts.Admin + "," + RolesConsts.Dater)]
public class ChatApplicationService : ApplicationService, IChatApplicationService
{
    private readonly IChatRepository _chatRepository;
    private readonly IChatParticipantRepository _chatParticipantRepository;
    private readonly ICurrentMelodyMatchUser _currentMelodyMatchUser;
    
    public ChatApplicationService(
        IChatRepository chatRepository,
        IChatParticipantRepository chatParticipantRepository,
        ICurrentMelodyMatchUser currentMelodyMatchUser)
    {
        _chatRepository = chatRepository;
        _chatParticipantRepository = chatParticipantRepository;
        _currentMelodyMatchUser = currentMelodyMatchUser;
    }

    public async Task<PagedResultDto<ChatResponseDto>> GetUserChatsAsync(PagedResultRequestDto input)
    {
        var userId = await _currentMelodyMatchUser.GetIdAsync();
        var chats = await _chatRepository.GetUserChatsAsync(userId);

        var totalCount = chats.Count;

        chats = chats
            .OrderByDescending(x => x.LastModificationTime ?? x.CreationTime)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToList();

        var chatDtos = ObjectMapper.Map<List<Chats.Chat>, List<ChatResponseDto>>(chats);
        
        foreach (var chatDto in chatDtos)
        {
            var chat = chats.First(c => c.Id == chatDto.Id);
            chatDto.UnreadCount = chat.Messages.Count(m => m.SenderId != userId && !m.IsRead);
        }

        return new PagedResultDto<ChatResponseDto>(totalCount, chatDtos);
    }
    
    public async Task<ChatResponseDto> GetChatAsync(System.Guid chatId)
    {
        var chat = await _chatRepository.GetWithParticipantsAsync(chatId);
        
        if (chat == null)
        {
            throw new EntityNotFoundException(typeof(Chats.Chat), chatId);
        }

        return ObjectMapper.Map<Chats.Chat, ChatResponseDto>(chat);
    }
    
    public async Task<ChatResponseDto> CreateChatAsync(CreateChatRequestDto request)
    {
        var currentUserId = await _currentMelodyMatchUser.GetIdAsync();
        
        if (!request.UserIds.Contains(currentUserId))
        {
            request.UserIds.Add(currentUserId);
        }

        var chat = new Chats.Chat(request.UserIds);
        await _chatRepository.InsertAsync(chat, autoSave: true);

        return ObjectMapper.Map<Chats.Chat, ChatResponseDto>(chat);
    }
}