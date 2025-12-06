using System;
using System.Threading.Tasks;
using MelodyMatch.Notification.DTOs.Responses;
using Volo.Abp.Application.Services;

namespace MelodyMatch.Notification.Services;

public class NotificationSenderService : ApplicationService
{
    // This service will be overridden in HttpApi.Host to include SignalR functionality
    // For now, it's just a placeholder that can be called from application layer
    
    public virtual async Task SendNotificationToUserAsync(Guid userId, NotificationResponseDto notification)
    {
        // Base implementation - does nothing
        // The actual SignalR sending will be done in HttpApi.Host override
        await Task.CompletedTask;
    }
}

