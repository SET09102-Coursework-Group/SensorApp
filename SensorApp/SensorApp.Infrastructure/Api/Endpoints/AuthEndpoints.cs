using Microsoft.AspNetCore.Identity;
using SensorApp.Infrastructure.Api.Dtos;
using SensorApp.Infrastructure.Services.Auth;

namespace SensorApp.Infrastructure.Api.Endpoints;

/// <summary>
/// Maps the authentication endpoints.
/// The /login endpoint authenticates the user and returns a JWT token if successful.
/// </summary>
public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder routes, IConfiguration configuration)
    {
        routes.MapPost("/login", async (LoginDto loginDto, UserManager<IdentityUser> userManager, ITokenService tokenService) =>
        {
            var user = await userManager.FindByNameAsync(loginDto.Username);
            if (user is null || !await userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return Results.Unauthorized();
            }

            var roles = await userManager.GetRolesAsync(user);
            var claims = await userManager.GetClaimsAsync(user);

            var token = tokenService.GenerateToken(user, roles, claims);
            var response = new AuthResponseDto
            {
                UserId = user.Id,
                Username = user.UserName,
                Token = token
            };

            return Results.Ok(response);
        }).AllowAnonymous();

    }
}