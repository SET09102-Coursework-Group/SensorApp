using SensorApp.Tests.IntegrationTests.ApiEndpoints.AuthEndpoints;

namespace SensorApp.Tests.IntegrationTests.ApiEndpoints.Helpers;

public class TokenProvider
{
    private readonly HttpClient _client;

    public TokenProvider(HttpClient client)
    {
        _client = client;
    }

    public async Task<string> GetTokenAsync(string email, string password)
    {
        return await new TestUserBuilder(_client).WithCredentials(email, password).BuildTokenAsync();
    }

    public Task<string> GetAdminTokenAsync()
    {
        return GetTokenAsync(TestUsers.AdminEmail, TestUsers.AdminPassword);
    }

    public Task<string> GetOpsTokenAsync()
    {
        return GetTokenAsync(TestUsers.OpsEmail, TestUsers.OpsPassword);
    }
}

public static class TestUsers
{
    public const string AdminEmail = "admin@sensor.com";
    public const string AdminPassword = "MyP@ssword123";

    public const string OpsEmail = "ops@sensor.com";
    public const string OpsPassword = "MyP@ssword123";
}