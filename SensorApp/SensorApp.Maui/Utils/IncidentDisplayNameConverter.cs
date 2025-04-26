using System.Globalization;
using SensorApp.Shared.Dtos.Incident;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace SensorApp.Maui.Utils;

public class IncidentDisplayNameConverter : IValueConverter
{
    /// <summary>
    /// Converts an incident object to a display-friendly string representation.
    /// </summary>
    /// <param name="value">The value to convert, expected to be of type <see cref="IncidentDto"/>.</param>
    /// <param name="targetType">The target type of the conversion (not used in this implementation).</param>
    /// <param name="parameter">An optional parameter (not used in this implementation).</param>
    /// <param name="culture">The culture info used for the conversion (not used in this implementation).</param>
    /// <returns>A formatted string representing the incident's sensor type, sensor ID, incident type, and creation date.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IncidentDto incident)
        {
            var incidentTypeDisplayName = GetEnumDisplayName(incident.Type);
            return $"Sensor {incident.Sensor?.Id} ({incident.Sensor?.Type}) - {incidentTypeDisplayName} ({incident.Creation_date})";
        }

        return string.Empty;
    }
    private string GetEnumDisplayName(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttribute<DisplayAttribute>();
        return attribute?.Name ?? value.ToString();
    }

    /// <summary>
    /// Not implemented, as this converter is only used for one-way binding.
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
};
