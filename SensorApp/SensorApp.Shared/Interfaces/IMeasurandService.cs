using SensorApp.Shared.Models;

namespace SensorApp.Shared.Interfaces;

public interface IMeasurandService
{
    Task<List<MeasurandModel>> GetMeasurandsAsync(string token, int sensorId);
}
