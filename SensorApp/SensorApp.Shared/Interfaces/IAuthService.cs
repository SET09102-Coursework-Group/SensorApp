using SensorApp.Shared.Dtos;

namespace SensorApp.Shared.Interfaces;

/// <summary>
/// Defines the contract for an authentication service that handles user login
/// and provides status information about authentication attempts.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Attempts to authenticate a user by sending login credentials to the backend.
    /// </summary>
    /// <param name="loginDto">The user's login credentials (username and password).</param>
    /// <returns>
    /// An <see cref="AuthResponseDto"/> containing authentication results, including a JWT token if successful.
    /// </returns>
    Task<AuthResponseDto> Login(LoginDto loginDto);

    /// <summary>
    /// Gets a short status message about the most recent authentication attempt.
    /// Example messages include "Login Successful" or "Attempt failed".
    /// </summary>
    string StatusMessage { get; }
}
