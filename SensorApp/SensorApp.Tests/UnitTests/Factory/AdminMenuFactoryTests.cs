using FluentAssertions;
using SensorApp.Shared.Factories;
using SensorApp.Shared.Interfaces;

namespace SensorApp.Tests.UnitTests.Factory;

/// <summary>
/// Unit tests for <see cref="AdminMenuFactory"/> that provides the menu items for Administrator users once the admin logs in. 
/// </summary>
public class AdminMenuFactoryTests
{
    [Fact]
    public void CreateMenu_ShouldReturnExpectedMenuItems()
    {
        // Arrange
        AdminMenuFactory factory = new AdminMenuFactory();

        // Act
        var menu = factory.CreateMenu().ToList();

        // Assert
        menu.Should().NotBeNullOrEmpty("admin menu should have at least one item");

        menu.Should().ContainSingle(item =>item.Route == "AdminUsersPage");
    }
}