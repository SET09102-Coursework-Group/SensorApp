using SensorApp.Database.Models;
using SensorApp.Shared.Dtos;

namespace SensorApp.Api.Endpoints.Extensions;

public static class MeasurementDtoMap
{
    public static MeasurementDto ConvertToDto(this Measurement measurement)
    {
        return new MeasurementDto
        {
            Id = measurement.Id,
            Sensor_id = measurement.Sensor_id,
            Value = measurement.Value,
            Timestamp = measurement.Timestamp,
            Measurement_type_id = measurement.Measurement_type_id,
            MeasurementType = new MeasurandDto
            {
                Id = measurement.Measurement_type.Id,
                Name = measurement.Measurement_type.Name,
                Unit = measurement.Measurement_type.Unit,
                Min_safe_threshold = measurement.Measurement_type.Min_safe_threshold,
                Max_safe_threshold = measurement.Measurement_type.Max_safe_threshold
            }
        };
    }
}
