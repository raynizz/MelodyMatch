using System;
using System.ComponentModel;
using System.Reflection;
using Microsoft.Extensions.Localization;
using MelodyMatch.Localization;

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

    public static string GetLocalizedDescription(this Enum value, IStringLocalizer<MelodyMatchResource> localizer)
    {
        if (value == null)
        {
            return string.Empty;
        }

        var enumType = value.GetType();
        var enumName = enumType.Name;
        var valueName = value.ToString();
        
        // Try to get localized string with key pattern: "Enum:{EnumTypeName}:{EnumValueName}"
        var localizationKey = $"Enum:{enumName}:{valueName}";
        var localizedString = localizer[localizationKey];
        
        // If localization found, return it
        if (!localizedString.ResourceNotFound)
        {
            return localizedString.Value;
        }
        
        // Fallback to Description attribute
        return value.GetDescription();
    }
}