using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SensorApp.Maui.Utils;

/// <summary>
/// Converts Enum values to a user-friendly display name using the Display attribute if available.
/// Falls back to splitting PascalCase names if no Display attribute is present.
/// </summary>
public class EnumDisplayNameConverter : IValueConverter
{
    /// <summary>
    /// Timeout for regular expression operations to avoid performance issues.
    /// </summary> 
    private static readonly TimeSpan RegexTimeout = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Converts an Enum value to a display name.
    /// If a Display attribute is present, it uses the attribute's Name property.
    /// Otherwise, it formats the Enum name by inserting spaces before capital letters.
    /// </summary>
    /// <param name="value">The Enum value to convert.</param>
    /// <param name="targetType">The type of the target property (unused here).</param>
    /// <param name="parameter">Optional parameter (unused).</param>
    /// <param name="culture">The culture to use in the converter (unused here).</param>
    /// <returns>The user-friendly display name for the Enum value.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Enum enumValue)
        {
            var memberInfo = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();

            var displayAttribute = memberInfo?.GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute != null)
            {
                return displayAttribute.GetName();
            }

            // Fallback to split to pascalcase
            var fallbackRegex = new Regex("(?<!^)([A-Z])", RegexOptions.None, RegexTimeout);
            return fallbackRegex.Replace(enumValue.ToString(), " $1");

        }

        return value?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Converts a display name back to its corresponding Enum value.
    /// It first looks for a matching Display attribute, then tries by cleaning up the display name.
    /// This is not being used at the moment but needed to implement the IValueConverter interface
    /// </summary>
    /// <param name="value">The display name string to convert back to Enum.</param>
    /// <param name="targetType">The target Enum type.</param>
    /// <param name="parameter">Optional parameter (unused).</param>
    /// <param name="culture">The culture to use in the converter (unused here).</param>
    /// <returns>The Enum value corresponding to the provided display name.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string displayName && targetType.IsEnum)
        {
            foreach (var field in targetType.GetFields())
            {
                var displayAttribute = field.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute?.GetName() == displayName)
                {
                    return Enum.Parse(targetType, field.Name);
                }
            }

            var cleanupRegex = new Regex(@"\s+", RegexOptions.None, RegexTimeout);
            var enumName = cleanupRegex.Replace(displayName, "");
            return Enum.Parse(targetType, enumName);
        }

        return value!;
    }
}