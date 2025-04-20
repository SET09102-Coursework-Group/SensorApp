using SensorApp.Shared.Models;
using SensorApp.Shared.Services;
using SensorApp.Tests.TestHelpers;
using SensorApp.Tests.UnitTests.Factory;

namespace SensorApp.Tests.UnitTests.Services;

public class SensorAnalysisServiceTests
{
    private readonly SensorAnalysisService _service = new SensorAnalysisService();

    private static readonly float sensorLatitude = 40.7128f;
    private static readonly float sensorLongitude = 74.0060f;
    private MeasurandModel CreateTemperature() => MeasurandFactory.CreateMeasurand(9, "Temperature", "C", -10f, 40f);
    private MeasurandModel CreateHumidity() => MeasurandFactory.CreateMeasurand(10, "Relative humidity", "%", 80f, 100f);
    private SensorModel CreateSensorWithMeasurements(List<MeasurementModel> measurements) => SensorFactory.CreateSensor(1, "Weather", sensorLatitude, sensorLongitude, "Active", measurements);

    [Fact]
    public void GetLatestMeasurementsByType_ReturnsLatestMeasurementsByType()
    {
        // Arrange
        var temperature  = CreateTemperature();
        var humidity = CreateHumidity();
        List<MeasurementModel> measurements =
            [
                MeasurementFactory.CreateMeasurement(1, 10f, new DateTime(2024, 1, 1, 12, 00, 0), 9, temperature),
                MeasurementFactory.CreateMeasurement(1, 13f, new DateTime(2024, 2, 1, 13, 00, 0), 9, temperature),
                MeasurementFactory.CreateMeasurement(1, 12f, new DateTime(2024, 1, 1, 12, 00, 0), 10, humidity),
                MeasurementFactory.CreateMeasurement(1, 11f, new DateTime(2024, 2, 1, 13, 00, 0), 10, humidity)
            ];
        var sensor = CreateSensorWithMeasurements(measurements);

        // Act
        var latestMeasurements = _service.GetLatestMeasurementsByType(sensor);

        // Assert
        Assert.Equal(2, latestMeasurements.Count);
        Assert.True(latestMeasurements.TryGetValue(9, out var latestTemp));
        Assert.Equal(13f, latestTemp.Value);
        Assert.True(latestMeasurements.TryGetValue(10, out var latestHumidity));
        Assert.Equal(11f, latestHumidity.Value);
    }

    [Fact]
    public void GetLatestMeasurementsByType_ReturnsEmptyDictionary_WhenNoMeasurements()
    {
        //Arrange
        var sensor = CreateSensorWithMeasurements(new List<MeasurementModel>());
        
        //Act
        var result = _service.GetLatestMeasurementsByType(sensor);

        //Assert
        Assert.Empty(result);
    }

    [Fact]
    public void IsThresholdBreached_ReturnsTrue_WhenMeasurementExceedsThreshold()
    {
        //Arrange
        var temperature = CreateTemperature();
        List<MeasurementModel> measurements =
            [
                MeasurementFactory.CreateMeasurement(1, 50f, new DateTime(2024, 2, 1, 13, 00, 0), 9, temperature),
            ];
        var sensor = CreateSensorWithMeasurements(measurements);

        // Act
        var result = _service.IsThresholdBreached(sensor);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsThresholdBreached_ReturnsFalse_WhenAllMeasurementsAreSafe()
    {
        //Arrange
        var temperature = CreateTemperature();
        List<MeasurementModel> measurements =
            [
                MeasurementFactory.CreateMeasurement(1, 30f, new DateTime(2024, 2, 1, 13, 00, 0), 9, temperature),
            ];
        var sensor = CreateSensorWithMeasurements(measurements);


        // Act
        var result = _service.IsThresholdBreached(sensor);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsThresholdBreached_ReturnsFalse_WhenNoMeasurements()
    {
        //Arrange 
        var sensor = CreateSensorWithMeasurements(new List<MeasurementModel>());

        //Act
        var result = _service.IsThresholdBreached(sensor);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void GetLatestMeasurementsByType_HandlesManyMeasurementTypes()
    {
        //Arrange
        var measurements = new List<MeasurementModel>();

        for (int i = 0; i < 100; i++)
        {
            var measurand = MeasurandFactory.CreateMeasurand(i, $"Type {i}", "unit", 0f, 100f);
            measurements.Add(MeasurementFactory.CreateMeasurement(1, i, new DateTime(2024, 1, 1), i, measurand));
        }

        var sensor = CreateSensorWithMeasurements(measurements);

        //Act
        var result = _service.GetLatestMeasurementsByType(sensor);

        //Assert
        Assert.Equal(100, result.Count);
    }
}