using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MelodyMatch.Notification.DTOs.Responses;
using Volo.Abp.Application.Services;

namespace MelodyMatch.Notification.Services;

public interface INotificationApplicationService : IApplicationService
{
    Task<List<NotificationResponseDto>> GetMyNotificationsAsync(bool? isRead = null);
    
    Task<int> GetUnreadCountAsync();
    
    Task MarkAsReadAsync(Guid notificationId);
    
    Task MarkAllAsReadAsync();
}

