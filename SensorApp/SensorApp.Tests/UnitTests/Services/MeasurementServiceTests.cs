using SensorApp.Shared.Models;
using SensorApp.Shared.Services;
using System.Net;
using System.Text;
using System.Text.Json;

namespace SensorApp.Tests.UnitTests.Services;

/// <summary>
/// Unit tests for the <see cref="MeasurementService"/> class.
/// Validates measurement retrieval behavior under different API response scenarios.
/// </summary>
public class MeasurementServiceTests
{
    [Fact]
    public async Task GetMeasurementsAsync_ReturnsList_WhenSuccessfulResponse()
    {
        // Arrange
        var mockMeasurements = new List<MeasurementModel>
            {
                new() { Id = 1, Sensor_id = 1, Measurement_type_id = 9, Value = 22.5f, Timestamp = new DateTime(2025, 4, 20), MeasurementType = new MeasurandModel { Id = 9, Name = "Temperature", Unit = "C" } },
                new() { Id = 2, Sensor_id = 1, Measurement_type_id = 9, Value = 23.0f, Timestamp = new DateTime(2025, 4, 21), MeasurementType = new MeasurandModel { Id = 9, Name = "Temperature", Unit = "C" } }
            };

        var json = JsonSerializer.Serialize(mockMeasurements);
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        var httpClient = HttpClientTestFactory.Create(response);
        var service = new MeasurementService(httpClient);

        // Act
        var result = await service.GetMeasurementsAsync(sensorId: 1, measurandId: 9, from: new DateTime(2025, 4, 20), to: new DateTime(2025, 4, 21), token: "goodToken");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetMeasurementsAsync_ReturnsEmptyList_WhenUnauthorized_Following401Response()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        var httpClient = HttpClientTestFactory.Create(response);
        var service = new MeasurementService(httpClient);

        // Act
        var result = await service.GetMeasurementsAsync(token: "noToken");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetMeasurementsAsync_ReturnsEmptyList_WhenExceptionThrown()
    {
        // Arrange
        var httpClient = HttpClientTestFactory.CreateWithException(new HttpRequestException("Network failure"));
        var service = new MeasurementService(httpClient);

        // Act
        var result = await service.GetMeasurementsAsync(token: "anyToken");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}