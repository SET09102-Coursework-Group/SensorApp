using Microsoft.EntityFrameworkCore;
using SensorApp.Api.Endpoints.Extensions;
using SensorApp.Database.Data;
using SensorApp.Shared.Enums;

namespace SensorApp.Api.Endpoints;

/// <summary>
/// Defines API endpoints for retrieving measurement data from the system.
/// </summary>
public static class MeasurementEndpoint
{
    /// <summary>
    /// Maps HTTP GET endpoints related to measurement data.
    /// Provides filtered access to sensor measurements based on query parameters like sensor ID, measurand ID, and date range.
    /// </summary>
    /// <param name="routes">The application's route builder used to define API routes.</param>
    public static void MapMeasurementEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/measurements", async (SensorDbContext db, int? sensorId, int? measurementTypeId, DateTime? from, DateTime? to) =>
        {
            // Validate existence of sensor and measurement type before proceeding
            var validationResult = await ValidateRequestAsync(db, sensorId, measurementTypeId);
            if (validationResult is not null)
                return validationResult;

            // Query the database for measurements based on provided filters
            var measurements = await db.Measurement
                .Include(m => m.Measurement_type)
                .Include(m => m.Sensor)
                .ApplyFilters(sensorId, measurementTypeId, from, to)
                .OrderBy(m => m.Timestamp)
                .Select(m => m.ConvertToDto())
                .ToListAsync();

            return Results.Ok(measurements);

        })
        .RequireAuthorization(policy => policy.RequireRole(
            UserRole.EnvironmentalScientist.ToString(),
            UserRole.Administrator.ToString()));
    }

    /// <summary>
    /// Validates that the requested sensor and measurement type exist in the database.
    /// </summary>
    /// <param name="db">Database context.</param>
    /// <param name="sensorId">Sensor ID to validate.</param>
    /// <param name="measurementTypeId">Measurement type ID to validate</param>
    /// <returns>
    /// A failed <see cref="IResult"/> if validation fails, or null if validation passes.
    /// </returns>
    private static async Task<IResult?> ValidateRequestAsync(SensorDbContext db, int? sensorId, int? measurementTypeId)
    {
        if (sensorId is int sid && !await db.Sensors.AnyAsync(s => s.Id == sid))
        {
            return Results.NotFound($"Sensor with ID {sid} was not found.");
        }

        if (measurementTypeId is int mtid && !await db.Measurand.AnyAsync(mt => mt.Id == mtid))
        {
            return Results.NotFound($"Measurement type with ID {mtid} was not found.");
        }

        return null;
    }
}

