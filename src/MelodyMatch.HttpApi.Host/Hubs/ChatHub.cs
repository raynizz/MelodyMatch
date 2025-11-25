using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MelodyMatch.Chat.DTOs.Requests;
using MelodyMatch.Chat.Services;
using MelodyMatch.Chats;
using MelodyMatch.Contexts.MelodyMatchUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Volo.Abp.DependencyInjection;

namespace MelodyMatch.Hubs;

[Authorize]
public class ChatHub : Hub, ITransientDependency
{
    private readonly IMessageApplicationService _messageApplicationService;
    private readonly IChatParticipantRepository _chatParticipantRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly ICurrentMelodyMatchUser _currentMelodyMatchUser;
    
    private static readonly Dictionary<Guid, List<string>> UserConnections = new();
    private static readonly object Lock = new();

    public ChatHub(
        IMessageApplicationService messageApplicationService,
        IChatParticipantRepository chatParticipantRepository,
        IMessageRepository messageRepository,
        ICurrentMelodyMatchUser currentMelodyMatchUser)
    {
        _messageApplicationService = messageApplicationService;
        _chatParticipantRepository = chatParticipantRepository;
        _messageRepository = messageRepository;
        _currentMelodyMatchUser = currentMelodyMatchUser;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = await _currentMelodyMatchUser.GetIdAsync();
        
        lock (Lock)
        {
            if (!UserConnections.ContainsKey(userId))
            {
                UserConnections[userId] = new List<string>();
            }
            UserConnections[userId].Add(Context.ConnectionId);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = await _currentMelodyMatchUser.GetIdAsync();
        
        lock (Lock)
        {
            if (UserConnections.ContainsKey(userId))
            {
                UserConnections[userId].Remove(Context.ConnectionId);
                if (UserConnections[userId].Count == 0)
                {
                    UserConnections.Remove(userId);
                }
            }
        }

        await base.OnDisconnectedAsync(exception);
    }
    
    public async Task SendMessage(CreateMessageRequestDto request)
    {
        var currentUserId = await _currentMelodyMatchUser.GetIdAsync();
        
        if (!await _chatParticipantRepository.ExistsInChatAsync(request.ChatId, currentUserId))
        {
            throw new UnauthorizedAccessException("You are not a participant of this chat.");
        }

        var message = await _messageApplicationService.SendMessageAsync(request);

        var participants = await _chatParticipantRepository.GetByChatIdAsync(request.ChatId);
        var recipientIds = participants.Where(p => p.UserId != currentUserId).Select(p => p.UserId).ToList();

        await SendMessageToUsers(recipientIds, "ReceiveMessage", message);
    }
    
    public async Task MarkMessagesAsRead(Guid chatId, List<Guid> messageIds)
    {
        var currentUserId = await _currentMelodyMatchUser.GetIdAsync();
        
        if (!await _chatParticipantRepository.ExistsInChatAsync(chatId, currentUserId))
        {
            throw new UnauthorizedAccessException("You are not a participant of this chat.");
        }

        await _messageRepository.MarkMessagesAsReadAsync(messageIds, currentUserId);

        var participants = await _chatParticipantRepository.GetByChatIdAsync(chatId);
        var otherUserIds = participants.Where(p => p.UserId != currentUserId).Select(p => p.UserId).ToList();

        await SendMessageToUsers(otherUserIds, "MessagesRead", new { ChatId = chatId, MessageIds = messageIds, ReadBy = currentUserId });
    }
    
    public async Task UserTyping(Guid chatId)
    {
        var currentUserId = await _currentMelodyMatchUser.GetIdAsync();
        
        if (!await _chatParticipantRepository.ExistsInChatAsync(chatId, currentUserId))
        {
            return;
        }

        var participants = await _chatParticipantRepository.GetByChatIdAsync(chatId);
        var otherUserIds = participants.Where(p => p.UserId != currentUserId).Select(p => p.UserId).ToList();

        await SendMessageToUsers(otherUserIds, "UserTyping", new { ChatId = chatId, UserId = currentUserId });
    }
    
    public async Task UserStoppedTyping(Guid chatId)
    {
        var currentUserId = await _currentMelodyMatchUser.GetIdAsync();
        
        if (!await _chatParticipantRepository.ExistsInChatAsync(chatId, currentUserId))
        {
            return;
        }

        var participants = await _chatParticipantRepository.GetByChatIdAsync(chatId);
        var otherUserIds = participants.Where(p => p.UserId != currentUserId).Select(p => p.UserId).ToList();

        await SendMessageToUsers(otherUserIds, "UserStoppedTyping", new { ChatId = chatId, UserId = currentUserId });
    }
    
    private async Task SendMessageToUsers(List<Guid> userIds, string method, object data)
    {
        var connectionIds = new List<string>();
        
        lock (Lock)
        {
            foreach (var userId in userIds)
            {
                if (UserConnections.ContainsKey(userId))
                {
                    connectionIds.AddRange(UserConnections[userId]);
                }
            }
        }

        if (connectionIds.Any())
        {
            await Clients.Clients(connectionIds).SendAsync(method, data);
        }
    }
}