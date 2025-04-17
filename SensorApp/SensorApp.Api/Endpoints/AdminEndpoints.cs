using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SensorApp.Shared.Dtos.Admin;
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
        //GET all existing users
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


        // POST create new user
        routes.MapPost("/admin/users", async (CreateUserDto dto, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) =>
        {
            if (await userManager.FindByEmailAsync(dto.Email) != null)
            {
                return Results.Conflict("User already exists");
            }

            var user = new IdentityUser { UserName = dto.Username, Email = dto.Email };
            var create = await userManager.CreateAsync(user, dto.Password);


            if (!create.Succeeded)
            {
                return Results.BadRequest(create.Errors);
            }

            var addRole = await userManager.AddToRoleAsync(user, dto.Role);
            if (!addRole.Succeeded)
            {
                await userManager.DeleteAsync(user);
                return Results.BadRequest(addRole.Errors);
            }

            var resultDto = new UserWithRoleDto
            {
                Id = user.Id,
                Username = user.UserName!,
                Email = user.Email!,
                Role = dto.Role
            };
            return Results.Created($"/admin/users/{user.Id}", resultDto);

        }).RequireAuthorization(policy => policy.RequireRole(UserRole.Administrator.ToString()));
    }
}