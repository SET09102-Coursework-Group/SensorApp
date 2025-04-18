using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using SensorApp.Shared.Models;
using SensorApp.Maui.Interfaces;

namespace SensorApp.Maui.Helpers.Map;

public class SensorPinFactory : ISensorPinFactory
{
    public Pin CreatePin(SensorModel sensor)
    {
        var measurementsInfo = string.Join(" | ", sensor.LatestMeasurementsByType.Values.Select(m =>
            $"{m.MeasurementType?.Name}: {m.Value} ({m.Timestamp:t})"));

        return new Pin
        {
            Label = sensor.IsThresholdBreached
                ? $"!! {sensor.Type} - ALERT"
                : $"{sensor.Type} - {sensor.Status}",
            Location = new Location(sensor.Longitude, sensor.Latitude),
            Type = PinType.Place,
            Address = measurementsInfo
        };
    }
}