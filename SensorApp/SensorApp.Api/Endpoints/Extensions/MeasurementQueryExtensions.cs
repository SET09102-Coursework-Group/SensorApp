using SensorApp.Database.Models;

namespace SensorApp.Api.Endpoints.Extensions
{
    public static class MeasurementQueryExtensions
    {
        public static IQueryable<Measurement> ApplyFilters(this IQueryable<Measurement> query, int? sensorId, int? measurementTypeId, DateTime? from, DateTime? to)
        {
            if (sensorId != null)
            {
                query = query.Where(m => m.Sensor_id == sensorId);
            }

            if (measurementTypeId != null)
            {
                query = query.Where(m => m.Measurement_type_id == measurementTypeId);
            }

            if (from != null)
            {
                query = query.Where(m => m.Timestamp >= from.Value);
            }

            if (to != null)
            {
                query = query.Where(m => m.Timestamp <= to.Value);

            }

            return query;
        }
    }
}
