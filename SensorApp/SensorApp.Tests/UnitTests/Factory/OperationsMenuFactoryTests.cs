using FluentAssertions;
using SensorApp.Shared.Factories;
using SensorApp.Shared.Interfaces;

namespace SensorApp.Tests.UnitTests.Factory;
public class OperationsMenuFactoryTests
{
    [Fact]
    public void Operations_CreateMenu_ShouldReturnExpectedMenuItems()
    {
        // Arrange
        AdminMenuFactory factory = new AdminMenuFactory();

        // Act
        var menu = factory.CreateMenu().ToList();

        // Assert
        menu.Should().NotBeNullOrEmpty("Operations manager menu should have at least one item");

        menu.Should().Contain(item => item.Route == "SensorMapPage");
    }
}