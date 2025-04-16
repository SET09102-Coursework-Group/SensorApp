using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SensorApp.Shared.Dtos;
using SensorApp.Shared.Enums;

namespace SensorApp.Api.Endpoints;

/// <summary>
/// Contains admin-specific API endpoints for the application.
/// These endpoints are protected and only accessible by users with the Administrator role.
/// </summary>
public static class AdminEndpoints
{
    public static void MapAdminEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/admin/users", async (UserManager<IdentityUser> userManager) =>
        {
            var users = await userManager.Users.ToListAsync();
            var result = new List<UserWithRoleDto>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault();
                result.Add(new UserWithRoleDto
                {
                    Id = user.Id,
                    Username = user.UserName!,
                    Email = user.Email!,
                    Role = role!
                });
            }

            return Results.Ok(result);
        }).RequireAuthorization(policy => policy.RequireRole(UserRole.Administrator.ToString()));
    }
}
