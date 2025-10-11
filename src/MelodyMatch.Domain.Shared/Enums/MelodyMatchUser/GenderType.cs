using System.ComponentModel;

namespace MelodyMatch.Enums.MelodyMatchUser;

public enum GenderType
{
    [Description("Not Specified")]
    NotSpecified = 0,
    
    [Description("Male")]
    Male = 1,
    
    [Description("Female")]
    Female = 2
}