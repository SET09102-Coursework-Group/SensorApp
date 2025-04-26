using SensorApp.Shared.Dtos.Admin;
using System.Net.Http.Json;

namespace SensorApp.Tests.IntegrationTests.ApiEndpoints.Helpers;

/// <summary>
/// Helper methods for interacting with the User API during integration tests.
/// </summary>
public static class UserApiHelper
{
    /// <summary>
    /// Checks if a user exists in the system by calling the /admin/users endpoint.
    /// </summary>
    /// <param name="client">The HTTP client used to communicate with the API.</param>
    /// <param name="token">The JWT token used to authenticate the request.</param>
    /// <param name="userId">The ID of the user to search for.</param>
    /// <returns><c>true</c> if the user exists; otherwise, <c>false</c>.</returns>
    public static async Task<bool> UserExistsAsync(HttpClient client, string token, string userId)
    {
        var request = TestHelpers.CreateAuthorizedRequest("/admin/users", token, HttpMethod.Get);
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var users = await response.Content.ReadFromJsonAsync<List<UserWithRoleDto>>();
        return users!.Any(u => u.Id == userId);
    }
}

