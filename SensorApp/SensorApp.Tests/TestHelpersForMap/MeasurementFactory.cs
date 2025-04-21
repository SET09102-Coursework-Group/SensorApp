using SensorApp.Shared.Models;

namespace SensorApp.Tests.TestHelpersForMap;

public class MeasurementFactory()
{
    public static MeasurementModel CreateMeasurement(int sensorId, float value, DateTime timestamp, int measurementTypeId, MeasurandModel measurementType)
    {
        return new MeasurementModel
        {
            Sensor_id = sensorId,
            Value = value,
            Timestamp = timestamp,
            Measurement_type_id = measurementTypeId,
            MeasurementType = measurementType
        };
    }
}
