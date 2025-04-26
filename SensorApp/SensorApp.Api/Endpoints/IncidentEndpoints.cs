using Microsoft.AspNetCore.Identity;
using SensorApp.Shared.Dtos.Incident;
using SensorApp.Api.Interfaces;
using SensorApp.Shared.Enums;

namespace SensorApp.Api.Endpoints;

public static class IncidentEndpoints
{
    /// <summary>
    /// Maps the incident-related HTTP endpoints to the application route builder.
    /// </summary>
    /// <param name="routes">The route builder for registering HTTP endpoints.</param>
    public static void MapIncidentEndpoints(this IEndpointRouteBuilder routes)
    {
        var incidents = routes.MapGroup("/incident")
            .RequireAuthorization(policy => policy.RequireRole(
                UserRole.OperationsManager.ToString(),
                UserRole.Administrator.ToString()
            ));

        // GET endpoint to fetch all incidents
        incidents.MapGet("", async (IIncidentService service) =>
        {
            var incidents = await service.GetAllIncidentsAsync();
            return Results.Ok(incidents);
        });

        // POST endpoint to create a new incident
        incidents.MapPost("/create", async (
            HttpContext context,
            CreateIncidentDto dto,
            IIncidentService service,
            UserManager<IdentityUser> userManager) =>
        {
            var userId = userManager.GetUserId(context.User);

            if (dto.Type == null|| dto.Status == null || dto.SensorId <= 0)
            {
                return Results.BadRequest("Invalid data. Ensure Type, Status, and SensorId are provided.");
            }

            var createdIncident = await service.CreateIncidentAsync(dto, userId);

            if (createdIncident == null)
            {
                return Results.Problem("Failed to create incident. Please try again later.");
            }

            return Results.Ok(createdIncident);
        });

        // PUT endpoint to resolve an existing incident by its ID
        incidents.MapPut("/resolve/{id}", async (
            int id,
            IncidentResolutionDto dto,
            IIncidentService service) =>
        {
            var result = await service.ResolveIncidentAsync(id, dto);
            return result ? Results.Ok() : Results.NotFound();
        });

        // DELETE endpoint to delete an incident by its ID
        incidents.MapDelete("/delete/{id}", async (
            int id,
            IIncidentService service) =>
        {
            var result = await service.DeleteIncidentAsync(id);
            return result ? Results.Ok() : Results.NotFound();
        });

    }
}