using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MelodyMatch.Constants;
using MelodyMatch.Contexts.MelodyMatchUser;
using MelodyMatch.Notification.DTOs.Responses;
using MelodyMatch.Notification.Services;
using MelodyMatch.Notifications;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;

namespace MelodyMatch.Notification;

[Authorize(Roles = RolesConsts.Admin + "," + RolesConsts.Dater)]
public class NotificationApplicationService : ApplicationService, INotificationApplicationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ICurrentMelodyMatchUser _currentMelodyMatchUser;
    
    public NotificationApplicationService(
        INotificationRepository notificationRepository,
        ICurrentMelodyMatchUser currentMelodyMatchUser)
    {
        _notificationRepository = notificationRepository;
        _currentMelodyMatchUser = currentMelodyMatchUser;
    }
    
    public async Task<List<NotificationResponseDto>> GetMyNotificationsAsync(bool? isRead = null)
    {
        var currentUserId = await _currentMelodyMatchUser.GetIdAsync();
        var notifications = await _notificationRepository.GetUserNotificationsAsync(currentUserId, isRead);
        
        return ObjectMapper.Map<List<Notifications.Notification>, List<NotificationResponseDto>>(notifications);
    }
    
    public async Task<int> GetUnreadCountAsync()
    {
        var currentUserId = await _currentMelodyMatchUser.GetIdAsync();
        return await _notificationRepository.GetUnreadCountAsync(currentUserId);
    }
    
    public async Task MarkAsReadAsync(Guid notificationId)
    {
        await _notificationRepository.MarkAsReadAsync(notificationId);
    }
    
    public async Task MarkAllAsReadAsync()
    {
        var currentUserId = await _currentMelodyMatchUser.GetIdAsync();
        await _notificationRepository.MarkAllAsReadAsync(currentUserId);
    }
}

