using SensorApp.Database.Models;

namespace SensorApp.Api.Endpoints.Extensions
{
    /// <summary>
    /// Provides extension methods for applying query filters to Measurement entities.
    /// </summary>
    public static class MeasurementQueryExtensions
    {
        /// <summary>
        /// Applies optional filters to a Measurement query based on sensor ID, measurement type ID, and date range.
        /// </summary>
        /// <param name="query">The base IQueryable of Measurement entities.</param>
        /// <param name="sensorId">Optional sensor ID to filter by.</param>
        /// <param name="measurementTypeId">Optional measurement type ID to filter by.</param>
        /// <param name="from">Optional start date for the timestamp filter.</param>
        /// <param name="to">Optional end date for the timestamp filter.</param>
        /// <returns>An IQueryable of Measurement entities after applying the filters.</returns>
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
