using Microsoft.EntityFrameworkCore;
using SensorApp.Database.Data;
using SensorApp.Shared.Dtos;
using SensorApp.Shared.Enums;

namespace SensorApp.Api.Endpoints;

/// <summary>
/// Defines API endpoints related to measurand data retrieval for sensors.
/// </summary>
public static class MeasurandEndpoints
{
    /// <summary>
    /// Maps the HTTP GET endpoint for retrieving measurand types associated with a specific sensor.
    /// The endpoint is protected and requires an authorized user with specific roles.
    /// </summary>
    /// <param name="routes">The application's endpoint route builder used to map HTTP routes.</param>
    public static void MapMeasurandEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/sensors/{sensorId}/measurands", async (SensorDbContext db, int sensorId) =>
        {
            // Query the database for all distinct measurands related to the specified sensor
            var measurandsList = await db.Measurement.Where(m => m.Sensor_id == sensorId)
                                           .Select(m => new MeasurandTypesDto
                                           {
                                               Id = m.Measurement_type.Id,
                                               Name = m.Measurement_type.Name,
                                               Unit = m.Measurement_type.Unit
                                           }).Distinct().OrderBy(x => x.Name).ToListAsync();

            return Results.Ok(measurandsList);
        }).RequireAuthorization(policy => policy.RequireRole(UserRole.EnvironmentalScientist.ToString(), UserRole.Administrator.ToString()));
    }
}