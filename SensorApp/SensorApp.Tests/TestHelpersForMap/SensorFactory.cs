using SensorApp.Shared.Models;

namespace SensorApp.Tests.TestHelpersForMap;

public class SensorFactory()
{
    public static SensorModel CreateSensor(int id, string type, float latitude, float longitude, string status, List<MeasurementModel> measurements)
    {
        return new SensorModel
        {
            Id = id,
            Type = type,
            Latitude = latitude,
            Longitude = longitude,
            Status = status,
            Measurements = measurements
        };
    }
}

