using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using SensorApp.Shared.Models;
using SensorApp.Maui.Interfaces;

namespace SensorApp.Maui.Helpers.Map;

public class SensorPinFactory : ISensorPinFactory
{
    public Pin CreatePin(SensorModel sensor)
    {
        var measurementsInfo = string.Join(" | ", sensor.LatestMeasurementsByType.Values.Select(measurement =>
            $"{measurement.MeasurementType?.Name}: {measurement.Value} ({measurement.Timestamp:t})"));

        return new Pin
        {
            Label = sensor.IsThresholdBreached
                ? $"!! {sensor.Type} - ALERT"
                : $"{sensor.Type} - {sensor.Status}",
            Location = new Location(sensor.Latitude, sensor.Longitude),
            Type = PinType.Place,
            Address = measurementsInfo
        };
    }
}