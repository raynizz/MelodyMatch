using System;
using System.Linq;
using System.Threading.Tasks;
using MelodyMatch.Chats;
using MelodyMatch.Enums.Notifications;
using MelodyMatch.Notifications;
using MelodyMatch.Reactions;
using MelodyMatch.Users;
using Volo.Abp.Domain.Services;

namespace MelodyMatch.Services;

public class UserBanService : DomainService
{
    private readonly IUserBanRepository _userBanRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IReactionRepository _reactionRepository;
    private readonly IChatParticipantRepository _chatParticipantRepository;
    private readonly IChatRepository _chatRepository;
    private readonly IMessageRepository _messageRepository;

    public UserBanService(
        IUserBanRepository userBanRepository,
        INotificationRepository notificationRepository,
        IReactionRepository reactionRepository,
        IChatParticipantRepository chatParticipantRepository,
        IChatRepository chatRepository,
        IMessageRepository messageRepository)
    {
        _userBanRepository = userBanRepository;
        _notificationRepository = notificationRepository;
        _reactionRepository = reactionRepository;
        _chatParticipantRepository = chatParticipantRepository;
        _chatRepository = chatRepository;
        _messageRepository = messageRepository;
    }

    public async Task BanUserAsync(Guid userId, string reason, Guid? complaintId = null, DateTime? expiresAt = null, bool isPermanent = false)
    {
        // Create ban record
        var ban = new UserBan(userId, reason, complaintId, expiresAt, isPermanent);
        await _userBanRepository.InsertAsync(ban);

        // Delete all reactions from/to banned user
        var userReactions = await _reactionRepository.GetQueryableAsync();
        var reactionsToDelete = userReactions
            .Where(r => r.FromUserId == userId || r.ToUserId == userId)
            .ToList();
        
        foreach (var reaction in reactionsToDelete)
        {
            await _reactionRepository.DeleteAsync(reaction);
        }

        // Delete all chats and messages
        var chatParticipants = await _chatParticipantRepository.GetByUserIdAsync(userId);
        foreach (var participant in chatParticipants)
        {
            var chat = await _chatRepository.GetWithParticipantsAsync(participant.ChatId);
            if (chat != null)
            {
                // Delete all messages in the chat
                var messages = await _messageRepository.GetQueryableAsync();
                var chatMessages = messages.Where(m => m.ChatId == chat.Id).ToList();
                foreach (var message in chatMessages)
                {
                    await _messageRepository.DeleteAsync(message);
                }
                
                // Delete chat
                await _chatRepository.DeleteAsync(chat);
            }
        }
    }

    public async Task<Notifications.Notification> CreateComplaintResolutionNotificationAsync(
        Guid reporterId, 
        Guid complaintId, 
        bool wasBanned, 
        string reportedUserName,
        string? banReason = null)
    {
        var title = wasBanned ? "Скарга розглянута - Юзер забанений" : "Скарга розглянута - Юзер помилуваний";
        var message = wasBanned
            ? $"Ваша скарга була розглянута. Користувач {reportedUserName} був забанений. Причина: {banReason}"
            : $"Ваша скарга була розглянута. Користувач {reportedUserName} був помилуваний адміністратором.";
        
        var notificationType = wasBanned ? NotificationType.UserBanned : NotificationType.ComplaintDismissed;
        
        var notification = new Notifications.Notification(
            reporterId,
            notificationType,
            title,
            message,
            complaintId);
        
        return await _notificationRepository.InsertAsync(notification);
    }
    
    public async Task UnbanUserAsync(Guid userId, string? reason = null)
    {
        var dbSet = await _userBanRepository.GetQueryableAsync();
        var activeBans = dbSet
            .Where(x => x.UserId == userId && x.IsActive && !x.IsDeleted)
            .ToList();
        
        foreach (var ban in activeBans)
        {
            ban.IsActive = false;
            await _userBanRepository.UpdateAsync(ban);
        }
    }
}

