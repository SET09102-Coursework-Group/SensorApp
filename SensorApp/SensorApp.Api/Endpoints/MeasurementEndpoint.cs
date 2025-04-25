using Microsoft.EntityFrameworkCore;
using SensorApp.Api.Endpoints.Extensions;
using SensorApp.Database.Data;
using SensorApp.Shared.Enums;

namespace SensorApp.Api.Endpoints;

public static class MeasurementEndpoint
{
    public static void MapMeasurementEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/measurements", async (SensorDbContext db, int? sensorId, int? measurementTypeId, DateTime? from, DateTime? to) =>
        {
            if (sensorId is int sid && !await db.Sensors.AnyAsync(s => s.Id == sid))
                return Results.NotFound($"Sensor with ID {sid} was not found.");

            if (measurementTypeId is int mtid && !await db.Measurand.AnyAsync(mt => mt.Id == mtid))
                return Results.NotFound($"Measurement type with ID {mtid} was not found.");


            var data = await db.Measurement.Include(m => m.Measurement_type)
                                               .Include(m => m.Sensor).ApplyFilters(sensorId, measurementTypeId, from, to).OrderBy(m => m.Timestamp).Select(m => m.ConvertToDto()).ToListAsync();

            return Results.Ok(data);

        }).RequireAuthorization(policy => policy.RequireRole(UserRole.EnvironmentalScientist.ToString(), UserRole.Administrator.ToString()));
    }
}

