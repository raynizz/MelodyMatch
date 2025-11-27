using System;
using System.Linq;
using System.Threading.Tasks;
using MelodyMatch.Hubs;
using MelodyMatch.Notification.DTOs.Responses;
using MelodyMatch.Notification.Services;
using Microsoft.AspNetCore.SignalR;
using Volo.Abp.DependencyInjection;

namespace MelodyMatch.Services;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(NotificationSenderService))]
public class SignalRNotificationSenderService : NotificationSenderService
{
    private readonly IHubContext<NotificationHub> _notificationHubContext;
    
    public SignalRNotificationSenderService(IHubContext<NotificationHub> notificationHubContext)
    {
        _notificationHubContext = notificationHubContext;
    }
    
    public override async Task SendNotificationToUserAsync(Guid userId, NotificationResponseDto notification)
    {
        var connectionIds = NotificationHub.GetConnectionIds(userId);
        if (connectionIds.Any())
        {
            await _notificationHubContext.Clients
                .Clients(connectionIds)
                .SendAsync("ReceiveNotification", notification);
        }
    }
}

