using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SensorApp.Core.Services.Auth;
using SensorApp.Database.Data;
using SensorApp.Shared.Dtos;
using SensorApp.Shared.Enums;

namespace SensorApp.Api.Endpoints;

/// <summary>
/// 
/// Defines endpoints to fetch sensor data for the interactive map.
/// The endpoints are protected and require authentication, but are not specific to any one role.
/// </summary>
public static class SensorMapEndpoint
{

    /// <summary>
    /// This method maps the /sensors endpoints to the application.
    /// <param name="routes">The route builder used to define endpoints in the app.</param>
    /// </summary>
    public static void MapSensorEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/sensors", async (SensorDbContext db) =>
        {
            var sensors = await db.Sensors
                .Select(s => new SensorDto
                {
                    Id = s.Id,
                    Type = s.Type,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    Site_zone = s.Site_zone,
                    Status = s.Status,
                    Measurements = s.Measurements.Select(m => new MeasurementDto
                    {
                        Id = m.Id,
                        Sensor_id = m.Sensor_id,
                        Value = m.Value,
                        Timestamp = m.Timestamp,
                        Measurement_type_id = m.Measurement_type_id,
                        MeasurementType = new MeasurandDto
                        {
                            Id = m.Measurement_type.Id,
                            Name = m.Measurement_type.Name,
                            Unit = m.Measurement_type.Unit,
                            Min_safe_threshold = m.Measurement_type.Min_safe_threshold,
                            Max_safe_threshold = m.Measurement_type.Max_safe_threshold
                        }
                    }).ToList()
                })
                .ToListAsync();

            return Results.Ok(sensors);
        });
    }
}