using System.ComponentModel;

namespace MelodyMatch.Enums.Reactions;

public enum ReactionType
{
    [Description("Skip")]
    Skip = 0,
    
    [Description("Like")]
    Like = 1,
    
    [Description("Like with Message")]
    LikeWithMessage = 2,
    
    [Description("Report")]
    Report = 3
}