using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using SensorApp.Shared.Models;
using SensorApp.Shared.Interfaces;

namespace SensorApp.Maui.Helpers.Map;

public class SensorPinFactory : ISensorPinFactory
{
    private readonly ISensorAnalysisService _sensorAnalysisService;

    public SensorPinFactory(ISensorAnalysisService sensorAnalysisService)
    {
        _sensorAnalysisService = sensorAnalysisService;
    }
    public Pin CreatePin(SensorModel sensor)
    {
        var latestMeasurements = _sensorAnalysisService.GetLatestMeasurementsByType(sensor);
        var measurementsInfo = string.Join(" | ", latestMeasurements.Values.Select(m =>
            $"{m.MeasurementType?.Name}: {m.Value} ({m.Timestamp:t})"));

        return new Pin
        {
            Label = _sensorAnalysisService.IsThresholdBreached(sensor)
                ? $"!! {sensor.Type} - ALERT"
                : $"{sensor.Type} - {sensor.Status}",
            Location = new Location(sensor.Latitude, sensor.Longitude),
            Type = PinType.Place,
            Address = measurementsInfo
        };

    }
}