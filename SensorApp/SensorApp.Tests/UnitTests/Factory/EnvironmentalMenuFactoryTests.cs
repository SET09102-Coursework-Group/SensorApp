using FluentAssertions;
using SensorApp.Shared.Factories;
using SensorApp.Shared.Interfaces;

namespace SensorApp.Tests.UnitTests.Factory;
public class EnvironmentalMenuFactoryTests
{
    [Fact]
    public void Environmental_CreateMenu_ShouldReturnExpectedMenuItems()
    {
        // Arrange
        IMenuFactory factory = new EnvironmentalMenuFactory();

        // Act
        var menu = factory.CreateMenu().ToList();

        // Assert
        menu.Should().NotBeNullOrEmpty("Environmental scientist menu should have at least one item");

        menu.Should().Contain(item => item.Route == "SensorMapPage");
    }
}