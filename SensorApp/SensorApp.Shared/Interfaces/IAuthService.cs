using SensorApp.Shared.Dtos;

namespace SensorApp.Shared.Interfaces;

/// <summary>
/// Defines the contract for an authentication service that handles user login
/// and provides status information about authentication attempts.
/// </summary>
public interface IAuthService
{
    Task<AuthResponseDto> Login(LoginDto loginDto);
    string StatusMessage { get; }
}
