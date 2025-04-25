using Microsoft.AspNetCore.Identity;
using SensorApp.Shared.Dtos.Incident;
using SensorApp.Api.Services.Interfaces;

namespace SensorApp.Api.Endpoints;

public static class IncidentEndpoints
{
    /// <summary>
    /// Maps the incident-related HTTP endpoints to the application route builder.
    /// </summary>
    /// <param name="routes">The route builder for registering HTTP endpoints.</param>
    public static void MapIncidentEndpoints(this IEndpointRouteBuilder routes)
    {
        // GET endpoint to fetch all incidents
        routes.MapGet("/incidents", async (IIncidentService service) =>
        {
            var incidents = await service.GetAllIncidentsAsync();
            return Results.Ok(incidents);
        });

        // POST endpoint to create a new incident
        routes.MapPost("/incident/create", async (
            HttpContext context,
            CreateIncidentDto dto,
            IIncidentService service,
            UserManager<IdentityUser> userManager) =>
        {
            var userId = userManager.GetUserId(context.User);

            if (string.IsNullOrEmpty(dto.Type) || string.IsNullOrEmpty(dto.Status) || dto.SensorId <= 0)
            {
                return Results.BadRequest("Invalid data. Ensure Type, Status, and SensorId are provided.");
            }

            await service.CreateIncidentAsync(dto, userId);
            return Results.Ok();
        });

        // PUT endpoint to resolve an existing incident by its ID
        routes.MapPut("/incident/resolve/{id}", async (
            int id,
            IncidentResolutionDto dto,
            IIncidentService service) =>
        {
            var result = await service.ResolveIncidentAsync(id, dto);
            return result ? Results.Ok() : Results.NotFound();
        });

        // DELETE endpoint to delete an incident by its ID
        routes.MapDelete("/incident/delete/{id}", async (
            int id,
            IIncidentService service) =>
        {
            var result = await service.DeleteIncidentAsync(id);
            return result ? Results.Ok() : Results.NotFound();
        });

    }
}