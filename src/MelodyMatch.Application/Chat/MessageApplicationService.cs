using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MelodyMatch.Chat.DTOs.Requests;
using MelodyMatch.Chat.DTOs.Responses;
using MelodyMatch.Chat.Services;
using MelodyMatch.Chats;
using MelodyMatch.Constants;
using MelodyMatch.Contexts.MelodyMatchUser;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;

namespace MelodyMatch.Chat;

[Authorize(Roles = RolesConsts.Admin + "," + RolesConsts.Dater)]
public class MessageApplicationService: ApplicationService, IMessageApplicationService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IChatParticipantRepository _chatParticipantRepository;
    private readonly ICurrentMelodyMatchUser _currentMelodyMatchUser;
    
    public MessageApplicationService(
        IMessageRepository messageRepository,
        IChatParticipantRepository chatParticipantRepository,
        ICurrentMelodyMatchUser currentMelodyMatchUser)
    {
        _messageRepository = messageRepository;
        _chatParticipantRepository = chatParticipantRepository;
        _currentMelodyMatchUser = currentMelodyMatchUser;
    }
    
    public async Task<List<MessageResponseDto>> GetMessagesAsync(Guid chatId)
    {
        var messages = await _messageRepository.GetMessagesByChatIdAsync(chatId);
        
        return ObjectMapper.Map<List<Message>, List<MessageResponseDto>>(messages);
    }
    
    public async Task<List<MessageResponseDto>> GetUnreadMessagesAsync()
    {
        var userId = await _currentMelodyMatchUser.GetIdAsync();
        var messages = await _messageRepository.GetUnreadMessagesAsync(userId);
        
        return ObjectMapper.Map<List<Message>, List<MessageResponseDto>>(messages);
    }
    
    public async Task<MessageResponseDto> SendMessageAsync(CreateMessageRequestDto request)
    {
        var currentUserId = await _currentMelodyMatchUser.GetIdAsync();

        // TODO: Add custom exception for unauthorized access
        if (!await _chatParticipantRepository.ExistsInChatAsync(request.ChatId, currentUserId))
        {
            throw new UnauthorizedAccessException("You are not a participant of this chat.");
        }

        var message = new Message(currentUserId, request.Content, false, request.ChatId);

        await _messageRepository.InsertAsync(message, autoSave: true);
        return ObjectMapper.Map<Message, MessageResponseDto>(message);
    }
    
    public async Task<MessageResponseDto> UpdateMessageAsync(UpdateMessageRequestDto request)
    {
        var message = await _messageRepository.GetAsync(request.Id);
        var currentUserId = await _currentMelodyMatchUser.GetIdAsync();

        if (message.SenderId != currentUserId)
        {
            throw new UnauthorizedAccessException("You can only update your own messages.");
        }
        
        message.Content = request.Content;
        message.IsRead = request.IsRead;
        
        await _messageRepository.UpdateAsync(message, autoSave: true);

        // Отримати оновлене повідомлення з відправником для відповіді
        var updatedMessage = await _messageRepository.GetByIdWithSenderAsync(request.Id);
        
        return ObjectMapper.Map<Message, MessageResponseDto>(updatedMessage);
    }

    public async Task DeleteMessageAsync(Guid messageId)
    {
        var userId = await _currentMelodyMatchUser.GetIdAsync();
        var message = await _messageRepository.GetAsync(messageId);
        
        if (message.SenderId != userId)
        {
            throw new UnauthorizedAccessException("You can only delete your own messages.");
        }
        
        await _messageRepository.DeleteAsync(messageId, autoSave: true);
    }
}