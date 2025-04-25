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
                var dto = await ToUserWithRoleDtoAsync(user, userManager);
                result.Add(dto);
            }
            return Results.Ok(result);

        }).RequireAuthorization(policy => policy.RequireRole(UserRole.Administrator.ToString()));

        // GET user by Id
        routes.MapGet("/admin/users/{id}", async (string id, UserManager<IdentityUser> userManager) =>
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return Results.NotFound();
            }

            var dto = await ToUserWithRoleDtoAsync(user, userManager);
            return Results.Ok(dto);

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

            var roleName = dto.Role.ToString();
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
            var addRole = await userManager.AddToRoleAsync(user, roleName);
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

        //DELETE user by Id
        routes.MapDelete("/admin/users/{id}", async (string id, UserManager<IdentityUser> userManager, HttpContext httpContext) =>
        {
            var currentUserId = userManager.GetUserId(httpContext.User);

            if (id == currentUserId)
            {
                return Results.BadRequest("You cannot delete your own account.");
            }

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return Results.NotFound();

            }

            var deleteResult = await userManager.DeleteAsync(user);
            if (!deleteResult.Succeeded)
            {
                return Results.BadRequest(deleteResult.Errors);
            }

            return Results.NoContent();
        }).RequireAuthorization(policy => policy.RequireRole(UserRole.Administrator.ToString()));

        // PUT update user by ID
        routes.MapPut("/admin/users/{id}", async (string id, UpdateUserDto dto, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, HttpContext httpContext) =>
        {
            var currentUserId = userManager.GetUserId(httpContext.User);
            if (currentUserId == id)
            {
                return Results.BadRequest("You cannot update your own account.");
            }

            //We need to make sure that the username and email are unique in the database
            var duplicateUsername = await userManager.Users.AnyAsync(u => u.Id != id && string.Equals(u.UserName, dto.Username, StringComparison.OrdinalIgnoreCase));
            var duplicateEmail = await userManager.Users.AnyAsync(u => u.Id != id && string.Equals(u.Email, dto.Email, StringComparison.OrdinalIgnoreCase));

            if (duplicateUsername)
            {
                return Results.Conflict("Username is already in use.");
            }

            if (duplicateEmail)
            {
                return Results.Conflict("Email is already in use.");
            }

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return Results.NotFound();
            }

            if (user.UserName != dto.Username || user.Email != dto.Email)   //change email or username if they change from what we have in the db
            {
                user.UserName = dto.Username;
                user.Email = dto.Email;
                var updateResult = await userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return Results.BadRequest(updateResult.Errors);
                }
            }

            var currentRoles = await userManager.GetRolesAsync(user);      //update roles if different from db
            var newRole = dto.Role.ToString();

            if (!currentRoles.Contains(newRole))
            {
                var removeResult = await userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                {
                    return Results.BadRequest(removeResult.Errors);
                }

                var addResult = await userManager.AddToRoleAsync(user, newRole);
                if (!addResult.Succeeded)
                {
                    return Results.BadRequest(addResult.Errors);
                }
            }


            if (!string.IsNullOrWhiteSpace(dto.Password))             //we update the password if provided only
            {
                var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await userManager.ResetPasswordAsync(user, resetToken, dto.Password);
                if (!passwordResult.Succeeded)
                {
                    return Results.BadRequest(passwordResult.Errors);
                }
            }

            return Results.NoContent();
        }).RequireAuthorization(policy => policy.RequireRole(UserRole.Administrator.ToString()));
    }


    public static async Task<UserWithRoleDto> ToUserWithRoleDtoAsync(IdentityUser user, UserManager<IdentityUser> userManager)
    {
        var roles = await userManager.GetRolesAsync(user);
        var roleName = roles.FirstOrDefault() ?? throw new InvalidOperationException($"User '{user.UserName}' does not have any assigned roles.");

        if (!Enum.TryParse<UserRole>(roleName, out var roleEnum))
        {
            throw new ArgumentException($"Invalid role name: {roleName}", nameof(roleName));
        }

        return new UserWithRoleDto
        {
            Id = user.Id,
            Username = user.UserName!,
            Email = user.Email!,
            Role = roleEnum
        };
    }
}