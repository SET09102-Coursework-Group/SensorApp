using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace SensorApp.Maui.Utils;

/// <summary>
/// A value converter that converts null or empty strings to a placeholder value ("--").
/// This is useful for displaying a default placeholder in the UI when a value is null or empty.
/// </summary>
public class NullToPlaceholderConverter : IValueConverter
{
    /// <summary>
    /// Converts a null or empty string to a placeholder value ("--").
    /// If the value is not null or empty, it returns the original value.
    /// </summary>
    /// <param name="value">The value to be converted (could be null or empty).</param>
    /// <param name="targetType">The type of the binding target property (not used here).</param>
    /// <param name="parameter">Additional parameter for the conversion (not used here).</param>
    /// <param name="culture">Culture information (not used here).</param>
    /// <returns>
    /// The placeholder string "--" if the value is null or empty, otherwise returns the original value.
    /// </returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.IsNullOrWhiteSpace(value?.ToString()) ? "--" : value;
    }

    /// <summary>
    /// Converts the placeholder back to the original value. 
    /// In this case, this method simply returns the value, as no special logic is needed for ConvertBack.
    /// </summary>
    /// <param name="value">The value to be converted (the placeholder or the original value).</param>
    /// <param name="targetType">The type of the binding target property (not used here).</param>
    /// <param name="parameter">Additional parameter for the conversion (not used here).</param>
    /// <param name="culture">Culture information (not used here).</param>
    /// <returns>
    /// The original value or the placeholder value (unchanged).
    /// </returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}
