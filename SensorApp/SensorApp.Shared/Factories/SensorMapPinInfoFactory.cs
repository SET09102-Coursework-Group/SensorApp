using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;

namespace SensorApp.Shared.Factories;

/// <summary>
/// Factory class responsible for creating <see cref="SensorPinInfo"/> objects used to display sensor data on a map.
/// It gathers information about the sensor's type, status, latest measurements, and whether any thresholds have been breached.
/// </summary>
public class SensorPinInfoFactory : ISensorPinInfoFactory
{
    private readonly ISensorAnalysisService _sensorAnalysisService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SensorPinInfoFactory"/> class.
    /// </summary>
    /// <param name="sensorAnalysisService">The service used to analyze sensor data and thresholds.</param>
    public SensorPinInfoFactory(ISensorAnalysisService sensorAnalysisService)
    {
        _sensorAnalysisService = sensorAnalysisService;
    }

    /// <summary>
    /// Creates a <see cref="SensorPinInfo"/> object that holds information about a specific sensor in preparation for creating Pin UI elements on an interactive map.
    /// The pin info includes details such as label, address, latitude, and longitude.
    /// </summary>
    /// <param name="sensor">The <see cref="SensorModel"/> for which the pin info is being created.</param>
    /// <returns>A <see cref="SensorPinInfo"/> object that contains the sensor information to be displayed on a map.</returns>
    public SensorPinInfo CreatePinInfo(SensorModel sensor)
    {
        var latestMeasurements = _sensorAnalysisService.GetLatestMeasurementsByType(sensor);

        var measurementsInfo = string.Join(" | ", latestMeasurements.Values.Select(m =>
            $"{m.MeasurementType?.Name}: {m.Value} ({m.Timestamp:HH:mm})"));

        // Determine the label for the pin, using an alert if the threshold is breached
        var label = _sensorAnalysisService.IsThresholdBreached(sensor)
            ? $"!! {sensor.Type} - ALERT" // If threshold is breached, show an alert label
            : $"{sensor.Type} - {sensor.Status}"; // Otherwise, show sensor type and status

        return new SensorPinInfo
        {
            Label = label,
            Address = measurementsInfo,
            Latitude = sensor.Latitude,
            Longitude = sensor.Longitude
        };
    }
};
