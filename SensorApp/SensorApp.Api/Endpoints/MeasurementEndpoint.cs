using Microsoft.EntityFrameworkCore;
using SensorApp.Database.Data;
using SensorApp.Database.Models;
using SensorApp.Shared.Dtos;
using SensorApp.Shared.Enums;

namespace SensorApp.Api.Endpoints;

public static class MeasurementEndpoint
{
    public static void MapMeasurementEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/measurements", async (SensorDbContext db, int? sensorId, int? measurementTypeId, DateTime? from, DateTime? to) =>
        {
            if (sensorId != null)
            {
                int sid = sensorId.Value;

                bool sensorExists = await db.Sensors.AnyAsync(s => s.Id == sid);
                if (!sensorExists)
                {
                    return Results.NotFound($"Sensor with ID {sid} was not found.");
                }
            }


            if (measurementTypeId != null)
            {
                int mtid = measurementTypeId.Value;

                bool measurementTypeExists = await db.Measurand.AnyAsync(mt => mt.Id == mtid);
                if (!measurementTypeExists)
                {
                    return Results.NotFound($"Measurement type with ID {mtid} was not found.");
                }
            }

            var entities = await db.Measurement.Include(m => m.Measurement_type)
                                               .Include(m => m.Sensor).ApplyFilters(sensorId, measurementTypeId, from, to).OrderBy(m => m.Timestamp).ToListAsync();

            var data = entities.Select(ConvertToDto).ToList();
            return Results.Ok(data);

        }).RequireAuthorization(policy => policy.RequireRole(UserRole.EnvironmentalScientist.ToString(), UserRole.Administrator.ToString()));
    }


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

    public static MeasurementDto ConvertToDto(Measurement measurement)
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

