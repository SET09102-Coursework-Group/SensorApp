using System.Globalization;

namespace SensorApp.Maui.Utils;

public class NullableIntConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value?.ToString() ?? string.Empty;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var s = (value as string)?.Trim();
        if (int.TryParse(s, out var i)) return i;
        return null;
    }
}