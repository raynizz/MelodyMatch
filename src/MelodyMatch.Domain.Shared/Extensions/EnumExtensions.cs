using System;
using System.ComponentModel;
using System.Reflection;

namespace MelodyMatch.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        if (value == null)
        {
            return string.Empty;
        }
        
        var field = value.GetType().GetField(value.ToString());
        if (field == null)
        {
            return string.Empty;
        }

        var attribute = field.GetCustomAttribute<DescriptionAttribute>();
        
        return attribute?.Description ?? string.Empty;
    }
}