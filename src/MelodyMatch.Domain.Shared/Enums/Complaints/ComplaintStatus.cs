using System.ComponentModel;

namespace MelodyMatch.Enums.Complaints;

public enum ComplaintStatus
{
    [Description("Sent")]
    Sent = 0,
    
    [Description("In Review")]
    InReview = 1,
    
    [Description("Resolved")]
    Resolved = 2,
    
    [Description("Dismissed")]
    Dismissed = 3
}