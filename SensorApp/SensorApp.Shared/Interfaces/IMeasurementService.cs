using SensorApp.Shared.Models;

namespace SensorApp.Shared.Interfaces;

public interface IMeasurementService
{
    Task<IReadOnlyList<MeasurementModel>> GetMeasurementsAsync(int? sensorId = null, int? measurandId = null, DateTime? from = null, DateTime? to = null, string token = "");
}