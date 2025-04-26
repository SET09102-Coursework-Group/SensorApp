using SensorApp.Shared.Models;

namespace SensorApp.Shared.Interfaces;
/// <summary>
/// Interface for retrieving measurands (measurable properties) associated with a specific sensor.
/// </summary>
public interface IMeasurandService
{
    Task<List<MeasurandModel>> GetMeasurandsAsync(string token, int sensorId);
}
