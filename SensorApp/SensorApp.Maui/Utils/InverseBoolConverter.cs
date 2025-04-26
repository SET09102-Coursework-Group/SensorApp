using System.Globalization;
using Microsoft.Maui.Controls;

namespace SensorApp.Maui.Utils;

/// <summary>
/// A value converter that inverts a boolean value.
/// This is useful for data binding scenarios where you need to toggle the boolean value in the UI.
/// </summary>
public class InverseBoolConverter : IValueConverter
{
    /// <summary>
    /// Converts a boolean value to its inverse.
    /// </summary>
    /// <param name="value">The value to be converted (should be a boolean).</param>
    /// <param name="targetType">The type of the binding target property (ot used in this implementation).</param>
    /// <param name="parameter">Additional parameter for the conversion (ot used in this implementation).</param>
    /// <param name="culture">Culture information (ot used in this implementation).</param>
    /// <returns>The inverted boolean value (true becomes false, and false becomes true).</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is bool b && !b;

    /// <summary>
    /// Converts the value back to its original state (inverse of inverse).
    /// Since it's the same operation, this method behaves like Convert().
    /// </summary>
    /// <param name="value">The value to be converted (should be a boolean).</param>
    /// <param name="targetType">The type of the binding target property (ot used in this implementation).</param>
    /// <param name="parameter">Additional parameter for the conversion (ot used in this implementation).</param>
    /// <param name="culture">Culture information (ot used in this implementation).</param>
    /// <returns>The inverted boolean value (true becomes false, and false becomes true).</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => value is bool b && !b;
}