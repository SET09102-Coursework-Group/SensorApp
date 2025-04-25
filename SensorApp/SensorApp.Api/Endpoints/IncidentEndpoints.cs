using Microsoft.AspNetCore.Identity;
using SensorApp.Shared.Dtos.Incident;
using SensorApp.Api.Services.Interfaces;

namespace SensorApp.Api.Endpoints;

public static class IncidentEndpoints
{
    public static void MapIncidentEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/incidents", async (IIncidentService service) =>
        {
            var incidents = await service.GetAllIncidentsAsync();
            return Results.Ok(incidents);
        });

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

        routes.MapPut("/incident/resolve/{id}", async (
            int id,
            IncidentResolutionDto dto,
            IIncidentService service) =>
        {
            var result = await service.ResolveIncidentAsync(id, dto);
            return result ? Results.Ok() : Results.NotFound();
        });

        routes.MapDelete("/incident/delete/{id}", async (
            int id,
            IIncidentService service) =>
        {
            var result = await service.DeleteIncidentAsync(id);
            return result ? Results.Ok() : Results.NotFound();
        });

    }
}