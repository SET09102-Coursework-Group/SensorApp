using SensorApp.Shared.Models;
using SensorApp.Shared.Services;
using System.Net;
using System.Text;
using System.Text.Json;

namespace SensorApp.Tests.UnitTests.Services;
public class MeasurandServiceTests
{
    [Fact]
    public async Task GetMeasurandsAsync_ReturnsList_WhenSuccessfulResponse()
    {
        // Arrange
        var mockMeasurands = new List<MeasurandModel>
        {
            new() { Id = 1, Name = "Temperature", Unit = "°C" },
            new() { Id = 2, Name = "Humidity", Unit = "%" }
        };

        var json = JsonSerializer.Serialize(mockMeasurands);
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        var httpClient = HttpClientTestFactory.Create(response);
        var service = new MeasurandService(httpClient);

        // Act
        var result = await service.GetMeasurandsAsync("validToken", 1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, m => m.Name == "Temperature");
        Assert.Contains(result, m => m.Name == "Humidity");
    }

    [Fact]
    public async Task GetMeasurandsAsync_ReturnsEmptyList_WhenUnauthorized()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        var httpClient = HttpClientTestFactory.Create(response);
        var service = new MeasurandService(httpClient);

        // Act
        var result = await service.GetMeasurandsAsync("badToken", 1);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetMeasurandsAsync_ReturnsEmptyList_WhenExceptionThrown()
    {
        // Arrange
        var httpClient = HttpClientTestFactory.CreateWithException(new HttpRequestException("Network failure"));
        var service = new MeasurandService(httpClient);

        // Act
        var result = await service.GetMeasurandsAsync("anyToken", 1);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}