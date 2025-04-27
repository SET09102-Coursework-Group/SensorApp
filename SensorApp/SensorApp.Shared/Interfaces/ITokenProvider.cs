namespace SensorApp.Shared.Interfaces;

/// <summary>
/// Defines a contract for retrieving authentication tokens used in secure API communication.
/// Implementations should safely fetch tokens, typically from a secure storage.
/// </summary>
public interface ITokenProvider
{
    /// <summary>
    /// Asynchronously retrieves the current authentication token.
    /// </summary>
    /// <returns>
    /// A JWT token string if available; otherwise, <c>null</c> if no token is stored or retrieval fails.
    /// </returns>
    Task<string?> GetTokenAsync();
}
