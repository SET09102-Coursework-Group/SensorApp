using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;

namespace SensorApp.Shared.Services;

/// <summary>
/// Provides functionality for analyzing sensor data, including checking for threshold breaches and retrieving the latest measurements by type.
/// </summary>
public class SensorAnalysisService : ISensorAnalysisService
{
    /// <summary>
    /// Gets the latest measurements for each measurement type for the specified sensor.
    /// </summary>
    /// <param name="sensor">The sensor for which the latest measurements are retrieved.</param>
    /// <returns>A dictionary where the key is the measurement type ID and the value is the corresponding <see cref="MeasurementModel"/>.</returns>
    public Dictionary<int, MeasurementModel> GetLatestMeasurementsByType(SensorModel sensor)
    {
        return sensor.Measurements
            .GroupBy(m => m.Measurement_type_id)
            .ToDictionary(
                g => g.Key,
                g => g.OrderByDescending(m => m.Timestamp).First());
    }

    /// <summary>
    /// Determines whether any of the thresholds for the specified sensor's measurements have been breached.
    /// </summary>
    /// <param name="sensor">The sensor to check for threshold breaches.</param>
    /// <returns>True if any threshold is breached; otherwise, false.</returns>
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
