using SensorApp.Shared.Models;

namespace SensorApp.Shared.Interfaces;
/// <summary>
/// Interface for retrieving measurement data from sensors, with optional filtering by sensor, measurand, and date range.
/// </summary>
public interface IMeasurementService
{
    Task<IReadOnlyList<MeasurementModel>> GetMeasurementsAsync(int? sensorId = null, int? measurandId = null, DateTime? from = null, DateTime? to = null, string token = "");
}