using SensorApp.Shared.Models;

namespace SensorApp.Shared.Interfaces;
/// <summary>
/// Interface for retrieving measurands (measurable properties) associated with a specific sensor.
/// </summary>
public interface IMeasurandService
{
    /// <summary>
    /// Retrieves a list of measurands available for a specific sensor.
    /// </summary>
    /// <param name="token">Authentication token required to authorize the request.</param>
    /// <param name="sensorId">The unique identifier of the sensor whose measurands are requested.</param>
    /// <returns>
    /// A list of <see cref="MeasurandModel"/> representing the measurable properties for the specified sensor.
    /// If no measurands are found or the request fails, an empty list is returned.
    /// </returns>
    Task<List<MeasurandModel>> GetMeasurandsAsync(string token, int sensorId);
}
