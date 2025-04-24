using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SensorApp.Database.Data;
using SensorApp.Shared.Dtos.Admin;
using SensorApp.Shared.Dtos;
using SensorApp.Shared.Dtos.Incident;
using SensorApp.Shared.Enums;

namespace SensorApp.Api.Endpoints;

public static class IncidentEndpoints
{
    public static void MapIncidentEndpoints(this IEndpointRouteBuilder routes)
    {
        //GET all existing users
        routes.MapGet("/incidents", async (SensorDbContext db, UserManager<IdentityUser> userManager) =>
        {
            var incidents = await db.Incidents
                .Select(i => new IncidentDto
                {
                    Id = i.Id,
                    Type = i.Type,
                    Status = i.Status,
                    Sensor = new SensorDto
                    {
                        Id = i.Sensor.Id,
                        Type = i.Sensor.Type,
                        Latitude = i.Sensor.Latitude,
                        Longitude = i.Sensor.Longitude,
                        Site_zone = i.Sensor.Site_zone,
                        Status = i.Sensor.Status
                    },
                    Sensor_id = i.Sensor_id,
                    Creation_date = i.Creation_date,
                    Priority = i.Priority,
                    Resolution_date = i.Resolution_date,
                    Responder_id = i.Responder_id,
                    Responder = new UserWithRoleDto
                    {
                        Id = i.Responder.Id,
                        Username = i.Responder.UserName,
                    },
                    Comments = i.Comments
                })
                .ToListAsync();

            return Results.Ok(incidents);

        });
        //.RequireAuthorization(policy => policy.RequireRole(UserRole.OperationsManager.ToString()));
    }
}