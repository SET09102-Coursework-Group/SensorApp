using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using SensorApp.Shared.Dtos.Incident;

namespace SensorApp.Maui.Utils;

public class IncidentDisplayNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IncidentDto incident)
        {
            return $"{incident.Sensor?.Type} Sensor {incident.Sensor?.Id} - {incident.Type} ({incident.Creation_date})";
        }

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
};