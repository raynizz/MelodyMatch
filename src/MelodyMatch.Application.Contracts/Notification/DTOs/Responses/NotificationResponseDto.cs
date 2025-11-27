using System;
using MelodyMatch.Enums.Notifications;
using Volo.Abp.Application.Dtos;

namespace MelodyMatch.Notification.DTOs.Responses;

public class NotificationResponseDto : FullAuditedEntityDto<Guid>
{
    public Guid UserId { get; set; }
    
    public NotificationType Type { get; set; }
    
    public string Title { get; set; }
    
    public string Message { get; set; }
    
    public bool IsRead { get; set; }
    
    public Guid? RelatedEntityId { get; set; }
}

