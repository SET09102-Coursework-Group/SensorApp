using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SensorApp.Infrastructure.Api.Dtos;

namespace SensorApp.Infrastructure.Api.Endpoints;

/// <summary>
/// Maps all admin-specific endpoints.
/// Authorization is required to be an Administrator. 
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
        }).RequireAuthorization(policy => policy.RequireRole("Administrator"));
    }
}
