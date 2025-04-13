using SensorApp.Maui.Factories.MenuFactory;
using SensorApp.Maui.Helpers.MenuRoles.Interfaces;

namespace SensorApp.Maui.Helpers.MenuRoles;

public static class MenuFactoryResolver
{
    public static IMenuFactory Resolve(UserRole role)
    {
        return role switch
        {
            UserRole.Administrator => new AdminMenuFactory(),
            _ => throw new NotSupportedException($"Role '{role}' is not supported."),
        };
    }
}