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
        
        bool wasOffline = false;
        lock (Lock)
        {
            if (!UserConnections.ContainsKey(userId))
            {
                UserConnections[userId] = new List<string>();
                wasOffline = true;
            }
            UserConnections[userId].Add(Context.ConnectionId);
        }

        await base.OnConnectedAsync();
        
        if (wasOffline)
        {
            await Clients.All.SendAsync("UserOnline", userId);
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = await _currentMelodyMatchUser.GetIdAsync();
        
        bool isNowOffline = false;
        lock (Lock)
        {
            if (UserConnections.ContainsKey(userId))
            {
                UserConnections[userId].Remove(Context.ConnectionId);
                if (UserConnections[userId].Count == 0)
                {
                    UserConnections.Remove(userId);
                    isNowOffline = true;
                }
            }
        }

        await base.OnDisconnectedAsync(exception);
        
        if (isNowOffline)
        {
            await Clients.All.SendAsync("UserOffline", userId);
        }
    }
    
    public Task<List<Guid>> GetOnlineUsers()
    {
        List<Guid> onlineUsers;
        lock (Lock)
        {
            onlineUsers = UserConnections.Keys.ToList();
        }
        return Task.FromResult(onlineUsers);
    }
    
    public Task<bool> IsUserOnline(Guid userId)
    {
        bool isOnline;
        lock (Lock)
        {
            isOnline = UserConnections.ContainsKey(userId);
        }
        return Task.FromResult(isOnline);
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
        var allUserIds = participants.Select(p => p.UserId).ToList();

        await SendMessageToUsers(allUserIds, "ReceiveMessage", message);
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
    
    public async Task UpdateMessage(UpdateMessageRequestDto request)
    {
        var currentUserId = await _currentMelodyMatchUser.GetIdAsync();
        
        if (!await _chatParticipantRepository.ExistsInChatAsync(request.ChatId, currentUserId))
        {
            throw new UnauthorizedAccessException("You are not a participant of this chat.");
        }

        var updatedMessage = await _messageApplicationService.UpdateMessageAsync(request);

        var participants = await _chatParticipantRepository.GetByChatIdAsync(request.ChatId);
        var allUserIds = participants.Select(p => p.UserId).ToList();

        await SendMessageToUsers(allUserIds, "MessageUpdated", updatedMessage);
    }
    
    public async Task DeleteMessage(Guid messageId, Guid chatId)
    {
        var currentUserId = await _currentMelodyMatchUser.GetIdAsync();
        
        if (!await _chatParticipantRepository.ExistsInChatAsync(chatId, currentUserId))
        {
            throw new UnauthorizedAccessException("You are not a participant of this chat.");
        }

        await _messageApplicationService.DeleteMessageAsync(messageId);

        var participants = await _chatParticipantRepository.GetByChatIdAsync(chatId);
        var allUserIds = participants.Select(p => p.UserId).ToList();

        await SendMessageToUsers(allUserIds, "MessageDeleted", new { MessageId = messageId, ChatId = chatId });
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