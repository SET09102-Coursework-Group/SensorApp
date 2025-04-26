using Microsoft.Extensions.Logging;
using SensorApp.Shared.Interfaces;

namespace SensorApp.Maui.Services;

/// <summary>
/// A service for retrieving the JWT token securely stored on the device.
/// Implements the <see cref="ITokenProvider"/> interface.
/// </summary>
public class TokenProvider : ITokenProvider
{
    private readonly ILogger<TokenProvider> _logger;

    public TokenProvider(ILogger<TokenProvider> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Attempts to retrieve the JWT token from secure storage.
    /// </summary>
    /// <returns>
    /// The JWT token as a string if retrieval is successful; otherwise, <c>null</c> if an error occurs.
    /// </returns>
    public async Task<string?> GetTokenAsync()
    {
        try
        {
            return await SecureStorage.GetAsync("Token");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve JWT from SecureStorage");
            return null;
        }
    }

}
