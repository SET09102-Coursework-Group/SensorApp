using FluentAssertions;
using SensorApp.Shared.Factories;

namespace SensorApp.Tests.UnitTests.Factory;
public class EnvironmentalMenuFactoryTests
{
    [Fact]
    public void Environmental_CreateMenu_ShouldReturnExpectedMenuItems()
    {
        // Arrange
        AdminMenuFactory factory = new AdminMenuFactory();

        // Act
        var menu = factory.CreateMenu().ToList();

        // Assert
        menu.Should().NotBeNullOrEmpty("Environmental scientist menu should have at least one item");

        menu.Should().Contain(item => item.Route == "SensorMapPage");
    }
}