using System;
using System.Threading.Tasks;
using MelodyMatch.Notification.DTOs.Responses;
using Volo.Abp.Application.Services;

namespace MelodyMatch.Notification;

public class NotificationSenderService : ApplicationService
{
    public virtual async Task SendNotificationToUserAsync(Guid userId, NotificationResponseDto notification)
    {
        await Task.CompletedTask;
    }
}

