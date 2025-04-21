using SensorApp.Shared.Models;

namespace SensorApp.Shared.Interfaces;

/// <summary>
/// Interface for analyzing sensor data, specifically for retrieving the latest measurements
/// and determining whether sensor thresholds have been breached.
/// </summary>
public interface ISensorAnalysisService
{
    /// <summary>
    /// Gets the latest measurements for each measurement type for the specified sensor.
    /// </summary>
    /// <param name="sensor">The sensor for which the latest measurements are retrieved.</param>
    /// <returns>A dictionary where the key is the measurement type ID and the value is the corresponding <see cref="MeasurementModel"/>.</returns>
    Dictionary<int, MeasurementModel> GetLatestMeasurementsByType(SensorModel sensor);
    /// <summary>
    /// Determines whether any of the thresholds for the specified sensor's measurements have been breached.
    /// </summary>
    /// <param name="sensor">The sensor to check for threshold breaches.</param>
    /// <returns>True if any threshold is breached; otherwise, false.</returns>
    bool IsThresholdBreached(SensorModel sensor);
}