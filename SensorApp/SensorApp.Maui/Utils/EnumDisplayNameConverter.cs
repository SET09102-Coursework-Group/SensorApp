using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SensorApp.Maui.Utils;


public class EnumDisplayNameConverter : IValueConverter
{
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
            return Regex.Replace(enumValue.ToString(), "(?<!^)([A-Z])", " $1");
        }

        return value?.ToString() ?? string.Empty;
    }

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

            var enumName = Regex.Replace(displayName, @"\s+", "");
            return Enum.Parse(targetType, enumName);
        }

        return value!;
    }
}