using SensorApp.Shared.Enums;
using SensorApp.Shared.Interfaces;

namespace SensorApp.Shared.Factories;

public static class MenuFactoryResolver
{
    /// <summary>
    /// Resolves the appropriate <see cref="IMenuFactory"/> implementation based on the user's role.
    ///
    /// For example:
    /// - An Administrator will get the <see cref="AdminMenuFactory"/>, which generates admin-specific menu items.
    /// 
    /// If the provided role is not recognized or not yet supported, the method throws an exception.
    /// </summary>
    /// <param name="role">The user's role</param>
    /// <returns>An instance of <see cref="IMenuFactory"/> responsible for generating the correct menu for the user</returns>
    public static IMenuFactory Resolve(UserRole role)
    {
        return role switch
        {
            UserRole.Administrator => new AdminMenuFactory(),
            UserRole.EnvironmentalScientist => new EnvironmentalMenuFactory(),
            UserRole.OperationsManager => new OperationsMenuFactory(),

            _ => throw new NotSupportedException($"Role '{role}' is not supported.")
        };
    }

}
