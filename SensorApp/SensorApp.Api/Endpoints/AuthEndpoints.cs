using Microsoft.AspNetCore.Identity;
using SensorApp.Core.Services.Auth;
using SensorApp.Shared.Dtos;

namespace SensorApp.Api.Endpoints;

/// <summary>
/// Defines the authentication API endpoints for login. 
/// These endpoints are publicly accessible and do not require prior authentication.
/// </summary>
public static class AuthEndpoints
{
    /// <summary>
    /// Maps authentication endpoints to the application's route builder.
    /// </summary>
    /// <param name="routes">The route builder used to define endpoints in the app.</param>
    public static void MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/login", async (LoginDto loginDto, UserManager<IdentityUser> userManager, ITokenService tokenService) =>
        {
            var user = await userManager.FindByNameAsync(loginDto.Username);
            if (user is null || !await userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return Results.Unauthorized();
            }

            var roles = await userManager.GetRolesAsync(user);

            var token = tokenService.GenerateToken(user, roles);
            var response = new AuthResponseDto
            {
                UserId = user.Id,
                Username = user.UserName!,
                Token = token
            };

            return Results.Ok(response);
        }).AllowAnonymous();
    }
}