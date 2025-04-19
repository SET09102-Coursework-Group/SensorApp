using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;

namespace SensorApp.Shared.Services;

public class SensorAnalysisService : ISensorAnalysisService
{
    public Dictionary<int, MeasurementModel> GetLatestMeasurementsByType(SensorModel sensor)
    {
        return sensor.Measurements
            .GroupBy(m => m.Measurement_type_id)
            .ToDictionary(
                g => g.Key,
                g => g.OrderByDescending(m => m.Timestamp).First());
    }

    public bool IsThresholdBreached(SensorModel sensor)
    {
        var latest = GetLatestMeasurementsByType(sensor);
        return latest.Values.Any(m =>
            m.MeasurementType != null &&
            m.MeasurementType.Min_safe_threshold.HasValue &&
            m.MeasurementType.Max_safe_threshold.HasValue &&
            (m.Value < m.MeasurementType.Min_safe_threshold || m.Value > m.MeasurementType.Max_safe_threshold));
    }
}
