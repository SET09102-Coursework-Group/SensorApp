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
        var admin = routes.MapGroup("/admin/users").RequireAuthorization(policy => policy.RequireRole(UserRole.Administrator.ToString()));

        admin.MapGet("", GetAllUsers);
        admin.MapGet("/{id}", GetUserById);
        admin.MapPost("", CreateUser);
        admin.MapDelete("/{id}", DeleteUser);
        admin.MapPut("/{id}", UpdateUser);
    }


    private static async Task<IResult> GetAllUsers(UserManager<IdentityUser> userManager)
    {
        var users = await userManager.Users.ToListAsync();
        var result = new List<UserWithRoleDto>(users.Count);
        foreach (var user in users)
        {
            result.Add(await ToUserWithRoleDtoAsync(user, userManager));
        }
        return Results.Ok(result);
    }

    private static async Task<IResult> GetUserById(string id, UserManager<IdentityUser> userManager)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(await ToUserWithRoleDtoAsync(user, userManager));
    }

    private static async Task<IResult> CreateUser(CreateUserDto dto, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
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
    }

    private static async Task<IResult> DeleteUser(string id, UserManager<IdentityUser> userManager, HttpContext httpContext)
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
        return deleteResult.Succeeded
            ? Results.NoContent()
            : Results.BadRequest(deleteResult.Errors);
    }

    private static async Task<IResult> UpdateUser(string id, UpdateUserDto dto, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, HttpContext httpContext)
    {
        var currentUserId = userManager.GetUserId(httpContext.User);
        if (currentUserId == id)
        {
            return Results.BadRequest("You cannot update your own account.");
        }

        var normalizedUsername = dto.Username.ToUpperInvariant();
        var duplicateUsername = await userManager.Users.AnyAsync(u => u.Id != id && u.NormalizedUserName == normalizedUsername);
        if (duplicateUsername)
        {
            return Results.Conflict("Username is already in use.");
        }

        var normalizedEmail = dto.Email.ToUpperInvariant();
        var duplicateEmail = await userManager.Users.AnyAsync(u => u.Id != id && u.NormalizedEmail == normalizedEmail);
        if (duplicateEmail)
        {
            return Results.Conflict("Email is already in use.");
        }

        var user = await userManager.FindByIdAsync(id);
        if (user == null)
        {
            return Results.NotFound();
        }

        if (user.UserName != dto.Username || user.Email != dto.Email)
        {
            user.UserName = dto.Username;
            user.Email = dto.Email;
            var updateResult = await userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return Results.BadRequest(updateResult.Errors);
            }
        }

        var currentRoles = await userManager.GetRolesAsync(user);
        var newRole = dto.Role.ToString();
        if (!currentRoles.Contains(newRole))
        {
            var remove = await userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!remove.Succeeded)
            {
                return Results.BadRequest(remove.Errors);
            }

            if (!await roleManager.RoleExistsAsync(newRole))
            {
                await roleManager.CreateAsync(new IdentityRole(newRole));
            }

            var add = await userManager.AddToRoleAsync(user, newRole);
            if (!add.Succeeded)
            {
                return Results.BadRequest(add.Errors);
            }
        }

        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var passwordResult = await userManager.ResetPasswordAsync(user, token, dto.Password);
            if (!passwordResult.Succeeded)
            {
                return Results.BadRequest(passwordResult.Errors);
            }
        }

        return Results.NoContent();
    }

    private static async Task<UserWithRoleDto> ToUserWithRoleDtoAsync(IdentityUser user,UserManager<IdentityUser> userManager)
    {
        var roles = await userManager.GetRolesAsync(user);
        var roleName = roles.FirstOrDefault()?? throw new InvalidOperationException($"User '{user.UserName}' has no role.");

        if (!Enum.TryParse<UserRole>(roleName, out var roleEnum))
        {
            var sanitized = roleName.Replace(" ", "");
            if (!Enum.TryParse<UserRole>(sanitized, out roleEnum)) throw new ArgumentException($"Invalid role: {roleName}", nameof(roleName));
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