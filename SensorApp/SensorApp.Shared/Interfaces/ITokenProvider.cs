namespace SensorApp.Shared.Interfaces;

/// <summary>
/// Interface for retrieving authentication tokens needed for secure API communication.
/// </summary>
public interface ITokenProvider
{
    Task<string?> GetTokenAsync();
}
