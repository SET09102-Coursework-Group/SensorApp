using System.Net;
using System.Text;
using System.Text.Json;
using SensorApp.Shared.Models;
using SensorApp.Shared.Services;

namespace SensorApp.Tests.UnitTests.Services;

public class SensorApiServiceTests
{
    [Fact]
    public async Task HappyPath_GetSensorsAsync_ReturnsListOfSensors_WhenSuccessful()
    {
        // Arrange
        var mockSensors = new List<SensorModel>
        {
            new SensorModel
            {
                Id = 1,
                Type = "Temperature",
                Latitude = 45.0f,
                Longitude = -122.0f,
                Status = "Active"
            }
        };

        var responseContent = new StringContent(
            JsonSerializer.Serialize(mockSensors),
            Encoding.UTF8,
            "application/json"
        );

        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = responseContent
        };

        var httpClient = HttpClientTestFactory.Create(response);
        var apiService = new SensorApiService(httpClient);

        // Act
        var result = await apiService.GetSensorsAsync("goodToken");

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Temperature", result[0].Type);
    }

    [Fact]
    public async Task GetSensorsAsync_ReturnsEmptyList_WhenUnauthorized_Following401Response()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);

        var httpClient = HttpClientTestFactory.Create(response);
        var apiService = new SensorApiService(httpClient);

        // Act
        var result = await apiService.GetSensorsAsync("unauthorizedToken");

        // Assert
        Assert.Empty(result); 
    }


    [Fact]
    public async Task GetSensorsAsync_ReturnsEmptyList_OnError()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

        var httpClient = HttpClientTestFactory.Create(response);
        var apiService = new SensorApiService(httpClient);

        // Act
        var result = await apiService.GetSensorsAsync("badToken");

        // Assert
        Assert.Empty(result);
    }

}
