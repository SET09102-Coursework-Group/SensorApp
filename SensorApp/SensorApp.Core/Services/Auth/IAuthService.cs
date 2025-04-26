using SensorApp.Shared.Dtos;

namespace SensorApp.Core.Services.Auth;

/// <summary>
/// Interface that defines authentication services for the application,
/// including user login and retrieval of authentication tokens.
/// </summary>
public interface IAuthService
{
    // <summary>
    /// Authenticates a user based on provided login credentials.
    /// </summary>
    /// <param name="login">The login details containing username and password.</param>
    /// <returns>
    /// An <see cref="AuthResponseDto"/> containing authentication result details and a token if successful;
    /// otherwise, null if authentication fails.
    /// </returns>
    Task<AuthResponseDto?> AuthenticateAsync(LoginDto login);
}
