using FluentAssertions;
using SensorApp.Shared.Enums;
using SensorApp.Shared.Factories;

namespace SensorApp.Tests.UnitTests.Factory;

/// <summary>
/// Unit tests for <see cref="MenuFactoryResolver"/>, which should return the correct menu factory based on the user's role once logged in
/// </summary>
public class MenuFactoryResolverTests
{
    [Fact]
    public void Resolve_ShouldReturn_AdminMenuFactory_WhenRoleIsAdministrator()
    {
        // Act
        var factory = MenuFactoryResolver.Resolve(UserRole.Administrator);

        // Assert
        factory.Should().BeOfType<AdminMenuFactory>();
    }

    [Fact]
    public void Resolve_ShouldReturn_EnvironmentalMenuFactory_WhenRoleIsEnvironmentalScientist()
    {
        // Act
        var factory = MenuFactoryResolver.Resolve(UserRole.EnvironmentalScientist);

        // Assert
        factory.Should().BeOfType<EnvironmentalMenuFactory>();
    }

    [Fact]
    public void Resolve_ShouldReturn_OperationsMenuFactory_WhenRoleIsOperationsManager()
    {
        // Act
        var factory = MenuFactoryResolver.Resolve(UserRole.OperationsManager);

        // Assert
        factory.Should().BeOfType<OperationsMenuFactory>();
    }

    [Fact]
    public void Resolve_ShouldThrowException_WhenRoleIsNotSupported()
    {
        // Arrange
        var unsupportedRole = (UserRole)123;

        // Act
        Action act = () => MenuFactoryResolver.Resolve(unsupportedRole);

        // Assert
        act.Should().Throw<NotSupportedException>();
    }
}