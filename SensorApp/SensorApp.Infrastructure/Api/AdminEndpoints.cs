using Microsoft.AspNetCore.Identity;

namespace SensorApp.Infrastructure.Api;

public static class AdminEndpoints
{
    public static void MapAdminEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/admin/users", async (UserManager<IdentityUser> userManager) =>
        {
            var users = userManager.Users.ToList();
            var result = new List<UserWithRoleDto>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault();
                result.Add(new UserWithRoleDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    Role = role
                });
            }

            return Results.Ok(result);
        }).RequireAuthorization(policy => policy.RequireRole("Administrator"));
    }
}

internal class UserWithRoleDto
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}
