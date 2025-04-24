using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace SensorApp.Maui.Utils;

public class NullToPlaceholderConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.IsNullOrWhiteSpace(value?.ToString()) ? "--" : value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}
