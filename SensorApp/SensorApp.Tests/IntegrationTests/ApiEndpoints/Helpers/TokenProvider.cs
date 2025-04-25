namespace SensorApp.Tests.IntegrationTests.ApiEndpoints.Helpers;

/// <summary>
/// Helper class to retrieve tokens for specific test users easily.
/// </summary>
public class TokenProvider
{
    private readonly HttpClient _client;

    public TokenProvider(HttpClient client)
    {
        _client = client;
    }

    /// <summary>
    /// Retrieves a JWT token for a specific user based on email and password.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <param name="password">The user's password.</param>
    /// <returns>A JWT token as a string.</returns>
    public async Task<string> GetTokenAsync(string email, string password)
    {
        return await new TestUserBuilder(_client).WithCredentials(email, password).BuildTokenAsync();
    }

    /// <summary>
    /// Retrieves a JWT token for the Admin test user.
    /// </summary>
    public Task<string> GetAdminTokenAsync()
    {
        return GetTokenAsync(TestUsers.AdminEmail, TestUsers.AdminPassword);
    }

    /// <summary>
    /// Retrieves a JWT token for the Operations (Ops) test user.
    /// </summary>
    public Task<string> GetOpsTokenAsync()
    {
        return GetTokenAsync(TestUsers.OpsEmail, TestUsers.OpsPassword);
    }
}

/// <summary>
/// Static class containing predefined test user credentials.
/// </summary>
public static class TestUsers
{
    public const string AdminEmail = "admin@sensor.com";
    public const string AdminPassword = "MyP@ssword123";

    public const string OpsEmail = "ops@sensor.com";
    public const string OpsPassword = "MyP@ssword123";
}