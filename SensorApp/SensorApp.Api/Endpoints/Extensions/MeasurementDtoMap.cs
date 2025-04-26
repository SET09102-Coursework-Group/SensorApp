using SensorApp.Database.Models;
using SensorApp.Shared.Dtos;

namespace SensorApp.Api.Endpoints.Extensions;

/// <summary>
/// Provides extension methods to map Measurement entities to MeasurementDto objects.
/// </summary>
public static class MeasurementDtoMap
{
    /// <summary>
    /// Converts a Measurement entity to a MeasurementDto.
    /// </summary>
    /// <param name="measurement">The Measurement entity to convert.</param>
    /// <returns>A populated MeasurementDto object.</returns>
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
