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
    /// <summary>
    /// Maps the admin endpoints related to user management under the route "/admin/users".
    /// Only users with the Administrator role can access these endpoints.
    /// </summary>
    /// <param name="routes">The endpoint route builder used to map the endpoints.</param>
    public static void MapAdminEndpoints(this IEndpointRouteBuilder routes)
    {
        var admin = routes.MapGroup("/admin/users").RequireAuthorization(policy => policy.RequireRole(UserRole.Administrator.ToString()));

        admin.MapGet("", GetAllUsers);
        admin.MapGet("/{id}", GetUserById);
        admin.MapPost("", CreateUser);
        admin.MapDelete("/{id}", DeleteUser);
        admin.MapPut("/{id}", UpdateUser);
    }

    /// <summary>
    /// Retrieves all users from the database along with their assigned roles.
    /// </summary>
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

    /// <summary>
    /// Retrieves a specific user by ID along with their role.
    /// </summary>
    private static async Task<IResult> GetUserById(string id, UserManager<IdentityUser> userManager)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(await ToUserWithRoleDtoAsync(user, userManager));
    }

    /// <summary>
    /// Creates a new user with the specified role.
    /// </summary>
    private static async Task<IResult> CreateUser(CreateUserDto dto, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        if (await userManager.FindByNameAsync(dto.Username) is not null)
        {
            return Results.Conflict("Username already exists");
        }
        
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

    /// <summary>
    /// Deletes a user by ID, with a check to prevent users from deleting themselves.
    /// </summary>
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

    /// <summary>
    /// Updates user information including username, email, role, and optionally password.
    /// Prevents updating the currently logged-in user's own account.
    /// </summary>
    private static async Task<IResult> UpdateUser(string id, UpdateUserDto dto, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, HttpContext httpContext)
    {
        var validationError = await ValidateUpdateRequestAsync(id, dto, userManager, httpContext);
        if (validationError is not null) return validationError;

        var user = await userManager.FindByIdAsync(id);
        if (user == null) return Results.NotFound();

        var coreUpdateError = await UpdateUsernameAndEmailAsync(user, dto, userManager);
        if (coreUpdateError is not null) return coreUpdateError;

        var roleUpdateError = await UpdateRoleAsync(user, dto.Role, userManager, roleManager);
        if (roleUpdateError is not null) return roleUpdateError;

        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            var pwError = await ResetPasswordAsync(user, dto.Password, userManager);
            if (pwError is not null) return pwError;
        }

        return Results.NoContent();
    }

    private static async Task<IResult?> ValidateUpdateRequestAsync(string id, UpdateUserDto dto, UserManager<IdentityUser> userManager, HttpContext httpContext)
    {
        if (userManager.GetUserId(httpContext.User) == id)
            return Results.BadRequest("You cannot update your own account.");

        var nameExists = await userManager.Users.AnyAsync(u =>
            u.Id != id && u.NormalizedUserName == dto.Username.ToUpperInvariant());
        if (nameExists) return Results.Conflict("Username is already in use.");

        var mailExists = await userManager.Users.AnyAsync(u =>
            u.Id != id && u.NormalizedEmail == dto.Email.ToUpperInvariant());
        return mailExists ? Results.Conflict("Email is already in use.") : null;
    }

    private static async Task<IResult?> UpdateUsernameAndEmailAsync(IdentityUser user, UpdateUserDto dto, UserManager<IdentityUser> userManager)
    {
        if (user.UserName == dto.Username && user.Email == dto.Email) return null;

        user.UserName = dto.Username;
        user.Email = dto.Email;

        var update = await userManager.UpdateAsync(user);
        return update.Succeeded ? null : Results.BadRequest(update.Errors);
    }

    private static async Task<IResult?> UpdateRoleAsync(IdentityUser user, UserRole newRoleEnum, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var newRole = newRoleEnum.ToString();
        var currentRoles = await userManager.GetRolesAsync(user);
        if (currentRoles.Contains(newRole)) return null;

        var remove = await userManager.RemoveFromRolesAsync(user, currentRoles);
        if (!remove.Succeeded) return Results.BadRequest(remove.Errors);

        if (!await roleManager.RoleExistsAsync(newRole))
            await roleManager.CreateAsync(new IdentityRole(newRole));

        var add = await userManager.AddToRoleAsync(user, newRole);
        return add.Succeeded ? null : Results.BadRequest(add.Errors);
    }

    private static async Task<IResult?> ResetPasswordAsync(IdentityUser user, string newPassword, UserManager<IdentityUser> userManager)
    {
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var result = await userManager.ResetPasswordAsync(user, token, newPassword);
        return result.Succeeded ? null : Results.BadRequest(result.Errors);
    }

    /// <summary>
    /// Helper method to convert an <see cref="IdentityUser"/> into a <see cref="UserWithRoleDto"/>,
    /// retrieving and validating the user's assigned role.
    /// </summary>
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