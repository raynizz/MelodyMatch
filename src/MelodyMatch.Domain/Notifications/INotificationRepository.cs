using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MelodyMatch.Notifications;

public interface INotificationRepository : IRepository<Notification, Guid>
{
    Task<List<Notification>> GetUserNotificationsAsync(Guid userId, bool? isRead = null);
    
    Task<int> GetUnreadCountAsync(Guid userId);
    
    Task MarkAsReadAsync(Guid notificationId);
    
    Task MarkAllAsReadAsync(Guid userId);
}

