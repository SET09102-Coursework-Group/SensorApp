using FluentAssertions;
using SensorApp.Shared.Factories;

namespace SensorApp.Tests.UnitTests.Factory;
public class OperationsMenuFactoryTests
{
    [Fact]
    public void Operations_CreateMenu_ShouldReturnExpectedMenuItems()
    {
        // Arrange
        AdminMenuFactory factory = new();

        // Act
        var menu = factory.CreateMenu().ToList();

        // Assert
        menu.Should().NotBeNullOrEmpty("Operations manager menu should have at least one item");

        menu.Should().Contain(item => item.Route == "SensorMapPage");
        menu.Should().Contain(item => item.Route == "IncidentList");
    }
}