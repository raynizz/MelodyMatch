using System.ComponentModel;

namespace MelodyMatch.Enums.Notifications;

public enum NotificationType
{
    [Description("Complaint Resolved")]
    ComplaintResolved = 0,
    
    [Description("Complaint Dismissed")]
    ComplaintDismissed = 1,
    
    [Description("User Banned")]
    UserBanned = 2,
    
    [Description("General")]
    General = 3,
    
    [Description("Complaint Received")]
    ComplaintReceived = 4
}

