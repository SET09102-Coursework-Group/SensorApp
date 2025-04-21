using Moq;
using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;
using SensorApp.Shared.Factories;
using SensorApp.Tests.TestHelpersForMap;

namespace SensorApp.Tests.UnitTests.Factory;
public class SensorPinInfoFactoryTests
{
    private readonly Mock<ISensorAnalysisService> _mockAnalysisService = new();
    private readonly SensorPinInfoFactory _factory;
    private const float sensorLatitude = 40.7128f;
    private const float sensorLongitude = 74.0060f;
    private SensorModel CreateSensor(List<MeasurementModel> measurements) => SensorFactory.CreateSensor(1, "Weather", sensorLatitude, sensorLongitude, "Active", measurements);

    public SensorPinInfoFactoryTests()
    {
        _factory = new SensorPinInfoFactory(_mockAnalysisService.Object);
    }

    [Fact]
    public void CreatePinInfo_ReturnsCorrectInfo_WhenNoThresholdBreached()
    {
        // Arrange
        var sensor = CreateSensor(new List<MeasurementModel>());
        var temperatureMeasurand = new MeasurandModel { Name = "Temperature" };
        var humidityMeasurand = new MeasurandModel { Name = "Relative humidity" };

        var measurementsDictionary = new Dictionary<int, MeasurementModel>
        {
            {
                9,
                new MeasurementModel
                {
                    Value = 22.5f,
                    Timestamp = new DateTime(2024, 4, 1, 14, 00, 0),
                    MeasurementType = temperatureMeasurand
                }
            },
            {
                10, new MeasurementModel
                {
                    Value = 60f,
                    Timestamp = new DateTime(2024, 4, 1, 14, 00, 0),
                    MeasurementType = humidityMeasurand
                }
            }
        };

        _mockAnalysisService.Setup(x => x.GetLatestMeasurementsByType(sensor))
            .Returns(measurementsDictionary);

        _mockAnalysisService.Setup(x => x.IsThresholdBreached(sensor))
            .Returns(false);

        // Act
        var result = _factory.CreatePinInfo(sensor);

        // Assert
        Assert.Equal("Weather - Active", result.Label);
        Assert.Contains("Temperature: 22.5 (14:00)", result.Address);
        Assert.Contains("Relative humidity: 60 (14:00)", result.Address);
        Assert.Equal(40.7128f, result.Latitude);
        Assert.Equal(74.0060f, result.Longitude);
    }

    [Fact]
    public void CreatePinInfo_AddsAlertToLabel_WhenThresholdBreached()
    {
        // Arrange
        var sensor = CreateSensor(new List<MeasurementModel>());

        _mockAnalysisService.Setup(x => x.GetLatestMeasurementsByType(sensor))
            .Returns(new Dictionary<int, MeasurementModel>());
        _mockAnalysisService.Setup(x => x.IsThresholdBreached(sensor))
            .Returns(true);

        // Act
        var result = _factory.CreatePinInfo(sensor);

        // Assert
        Assert.StartsWith("!!", result.Label);
        Assert.Contains("ALERT", result.Label);
    }

    [Fact]
    public void CreatePinInfo_WhenNoMeasurements_ReturnsEmptyAddress()
    {
        //Arrange
        var sensor = CreateSensor(new List<MeasurementModel>());
        _mockAnalysisService.Setup(x => x.GetLatestMeasurementsByType(sensor)).Returns(new Dictionary<int, MeasurementModel>());
        _mockAnalysisService.Setup(x => x.IsThresholdBreached(sensor)).Returns(false);

        var factory = new SensorPinInfoFactory(_mockAnalysisService.Object);

        // Act
        var result = factory.CreatePinInfo(sensor);

        //Assert
        Assert.Equal(string.Empty, result.Address);
    }
}
