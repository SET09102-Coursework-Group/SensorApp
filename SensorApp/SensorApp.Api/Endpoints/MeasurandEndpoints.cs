using Microsoft.EntityFrameworkCore;
using SensorApp.Database.Data;
using SensorApp.Shared.Enums;

namespace SensorApp.Api.Endpoints;

public static class MeasurandEndpoints
{
    public static void MapMeasurandEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/sensors/{sensorId}/measurands", async (SensorDbContext db, int sensorId) =>
        {
            var list = await db.Measurement.Where(m => m.Sensor_id == sensorId)
                                           .Select(m => new               
                                           {
                                              m.Measurement_type.Id,
                                              m.Measurement_type.Name,
                                              m.Measurement_type.Unit
                                           }).Distinct().OrderBy(x => x.Name).ToListAsync();

            return Results.Ok(list);
        }).RequireAuthorization(policy => policy.RequireRole(UserRole.EnvironmentalScientist.ToString(), UserRole.Administrator.ToString()));
    }
}