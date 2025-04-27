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
    /// Maps all authentication  endpoint to the API application.
    /// </summary>
    /// <param name="app">The endpoint route builder used to configure the application's endpoints.</param>
    /// <returns>The modified <see cref="IEndpointRouteBuilder"/> with authentication endpoints mapped.</returns>
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        // Define the /login POST endpoint.
        // This endpoint accepts login credentials (username and password) and attempts to authenticate the user.
        app.MapPost("/login", async (LoginDto dto, IAuthService auth) =>
        {
            var result = await auth.AuthenticateAsync(dto);
            return result is null
                ? Results.Unauthorized()
                : Results.Ok(result);

        }).AllowAnonymous().WithName("Login").WithTags("Authentication");

        return app;
    }
}