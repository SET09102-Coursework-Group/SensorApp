using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;

namespace SensorApp.Shared.Factories;

public class SensorPinInfoFactory : ISensorPinInfoFactory
{
    private readonly ISensorAnalysisService _sensorAnalysisService;

    public SensorPinInfoFactory(ISensorAnalysisService sensorAnalysisService)
    {
        _sensorAnalysisService = sensorAnalysisService;
    }

    public SensorPinInfo CreatePinInfo(SensorModel sensor)
    {
        var latestMeasurements = _sensorAnalysisService.GetLatestMeasurementsByType(sensor);
        var measurementsInfo = string.Join(" | ", latestMeasurements.Values.Select(m =>
            $"{m.MeasurementType?.Name}: {m.Value} ({m.Timestamp:t})"));

        var label = _sensorAnalysisService.IsThresholdBreached(sensor)
            ? $"!! {sensor.Type} - ALERT"
            : $"{sensor.Type} - {sensor.Status}";

        return new SensorPinInfo
        {
            Label = label,
            Address = measurementsInfo,
            Latitude = sensor.Latitude,
            Longitude = sensor.Longitude
        };
    }
};
