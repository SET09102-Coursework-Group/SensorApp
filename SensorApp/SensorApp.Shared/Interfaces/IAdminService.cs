using SensorApp.Shared.Dtos.Admin;

namespace SensorApp.Shared.Interfaces;

/// <summary>
/// Defines administrative operations related to user management, such as creating, retrieving, updating, and deleting users via the API.
/// </summary>
public interface IAdminService
{
    /// <summary>
    /// Retrieves the information of a specific user by their unique ID.
    /// </summary>
    /// <param name="token">The authentication token for authorization.</param>
    /// <param name="userId">The ID of the user to retrieve.</param>
    /// <returns>
    /// A <see cref="UserWithRoleDto"/> containing the user's information if found; otherwise, <c>null</c>.
    /// </returns>
    Task<UserWithRoleDto?> GetUserByIdAsync(string token, string userId);

    /// <summary>
    /// Retrieves all users registered in the system along with their assigned roles.
    /// </summary>
    /// <param name="token">The authentication token for authorization.</param>
    /// <returns>
    /// A list of <see cref="UserWithRoleDto"/> representing all users.
    /// </returns>
    Task<List<UserWithRoleDto>> GetAllUsersAsync(string token);

    /// <summary>
    /// Adds a new user to the system.
    /// </summary>
    /// <param name="token">The authentication token for authorization.</param>
    /// <param name="newUser">The information of the new user to create.</param>
    /// <returns>True if the user was successfully created; otherwise, false.</returns>
    Task<bool> AddUserAsync(string token, CreateUserDto newUser);

    /// <summary>
    /// Deletes an existing user from the system by their user ID.
    /// </summary>
    /// <param name="token">The authentication token for authorization.</param>
    /// <param name="userId">The ID of the user to delete.</param>
    /// <returns>True if the user was successfully deleted; otherwise, false.</returns>
    Task<bool> DeleteUserAsync(string token, string userId);

    /// <summary>
    /// Updates the details of an existing user.
    /// </summary>
    /// <param name="token">The authentication token for authorization.</param>
    /// <param name="userId">The ID of the user to update.</param>
    /// <param name="updatedUser">The updated user information.</param>
    /// <returns>True if the user was successfully updated; otherwise, false.</returns>
    Task<bool> UpdateUserAsync(string token, string userId, UpdateUserDto updatedUser);
}