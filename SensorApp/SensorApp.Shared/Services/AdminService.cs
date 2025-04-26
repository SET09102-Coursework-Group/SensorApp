using SensorApp.Shared.Dtos.Admin;
using SensorApp.Shared.Helpers;
using SensorApp.Shared.Interfaces;

namespace SensorApp.Shared.Services;

/// <summary>
/// Service class that handles administrative user operations,
/// including retrieving, creating, updating, and deleting users via API calls.
/// </summary>
public class AdminService(HttpClient httpClient) : IAdminService
{
    private readonly HttpClient _httpClient = httpClient;

    /// <summary>
    /// Retrieves a specific user's information, including their role, by their user ID.
    /// </summary>
    /// <param name="token">Authentication token required for authorization.</param>
    /// <param name="userId">The unique ID of the user to retrieve.</param>
    /// <returns>
    /// A <see cref="UserWithRoleDto"/> representing the user's information, or null if not found.
    /// </returns>
    public async Task<UserWithRoleDto?> GetUserByIdAsync(string token, string userId)
    {
        var req = HttpRequestHelper.Create(HttpMethod.Get, $"/admin/users/{userId}", token);
        return await HttpRequestHelper.SendAsync<UserWithRoleDto>(_httpClient, req);
    }

    /// <summary>
    /// Retrieves a list of all users registered in the system along with their roles.
    /// </summary>
    /// <param name="token">Authentication token required for authorization.</param>
    /// <returns>
    /// A list of <see cref="UserWithRoleDto"/> representing all users.
    /// </returns>
    public async Task<List<UserWithRoleDto>> GetAllUsersAsync(string token)
    {
        var request = HttpRequestHelper.Create(HttpMethod.Get, "/admin/users", token);
        return await HttpRequestHelper.SendAsync<List<UserWithRoleDto>>(_httpClient, request) ?? [];
    }

    /// <summary>
    /// Adds a new user to the system.
    /// </summary>
    /// <param name="token">Authentication token required for authorization.</param>
    /// <param name="newUser">The details of the new user to be created.</param>
    /// <returns>True if the user was successfully created; otherwise, false.</returns>
    public async Task<bool> AddUserAsync(string token, CreateUserDto newUser)
    {
        var request = HttpRequestHelper.Create(HttpMethod.Post, "/admin/users", token, newUser);
        return await HttpRequestHelper.SendAsync(_httpClient, request);
    }

    /// <summary>
    /// Deletes an existing user from the system by their user ID.
    /// </summary>
    /// <param name="token">Authentication token required for authorization.</param>
    /// <param name="userId">The unique ID of the user to delete.</param>
    /// <returns>True if the user was successfully deleted; otherwise, false.</returns
    public async Task<bool> DeleteUserAsync(string token, string userId)
    {
        var request = HttpRequestHelper.Create(HttpMethod.Delete, $"/admin/users/{userId}", token);
        return await HttpRequestHelper.SendAsync(_httpClient, request);
    }

    /// <summary>
    /// Updates an existing user's information.
    /// </summary>
    /// <param name="token">Authentication token required for authorization.</param>
    /// <param name="userId">The unique ID of the user to update.</param>
    /// <param name="updatedUser">The updated user details to apply.</param>
    /// <returns>True if the user was successfully updated; otherwise, false.</returns>
    public async Task<bool> UpdateUserAsync(string token, string userId, UpdateUserDto updatedUser)
    {
        var req = HttpRequestHelper.Create(HttpMethod.Put, $"/admin/users/{userId}", token, updatedUser);
        return await HttpRequestHelper.SendAsync(_httpClient, req);
    }
}