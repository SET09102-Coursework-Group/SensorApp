using SensorApp.Shared.Dtos;

namespace SensorApp.Core.Services.Auth;

/// <summary>
/// Defines authentication-related operations for users.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Authenticates a user using their login credentials.
    /// </summary>
    /// <param name="login">An object containing the user's username and password.</param>
    /// <returns>
    /// An <see cref="AuthResponseDto"/> containing user details and a JWT token if authentication succeeds;
    /// otherwise, <c>null</c> if the credentials are invalid.
    /// </returns>
    Task<AuthResponseDto?> AuthenticateAsync(LoginDto login);
}
