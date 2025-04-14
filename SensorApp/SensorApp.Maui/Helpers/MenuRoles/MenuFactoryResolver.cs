using SensorApp.Maui.Factories.MenuFactory;
using SensorApp.Maui.Helpers.MenuRoles.Interfaces;

namespace SensorApp.Maui.Helpers.MenuRoles;

/// <summary>
/// Resolves the proper menu factory based on the specified user role.
/// </summary>
public static class MenuFactoryResolver
{

    /// <summary>
    /// Returns the appropriate IMenuFactory implementation for the given user role.
    /// </summary>
    /// <param name="role">The user role to resolve a menu for.</param>
    /// <returns>An implementation of IMenuFactory.</returns>
    /// <exception cref="NotSupportedException">
    /// Thrown when a user role is not supported.
    /// </exception>
    public static IMenuFactory Resolve(UserRole role)
    {
        return role switch
        {
            UserRole.Administrator => new AdminMenuFactory(),
            _ => throw new NotSupportedException($"Role '{role}' is not supported."),
        };
    }
}