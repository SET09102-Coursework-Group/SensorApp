using SensorApp.Shared.Models;

namespace SensorApp.Shared.Interfaces;
/// <summary>
/// Interface for retrieving measurement data from sensors, with optional filtering by sensor, measurand, and date range.
/// </summary>
public interface IMeasurementService
{
    /// <summary>
    /// Asynchronously retrieves a list of measurements from the API.
    /// Supports optional filters by sensor ID, measurand ID, and a specific time range.
    /// </summary>
    /// <param name="sensorId">Optional sensor ID to filter measurements for a specific sensor.</param>
    /// <param name="measurandId">Optional measurand (measurement type) ID to filter by specific data type.</param>
    /// <param name="from">Optional start date to filter measurements recorded after this time.</param>
    /// <param name="to">Optional end date to filter measurements recorded before this time.</param>
    /// <param name="token">Authentication token required for authorized API access.</param>
    /// <returns>
    /// A read-only list of <see cref="MeasurementModel"/> representing the measurements retrieved from the API.
    /// Returns an empty list if no data is found or if the request fails.
    /// </returns>
    Task<IReadOnlyList<MeasurementModel>> GetMeasurementsAsync(int? sensorId = null, int? measurandId = null, DateTime? from = null, DateTime? to = null, string token = "");
}