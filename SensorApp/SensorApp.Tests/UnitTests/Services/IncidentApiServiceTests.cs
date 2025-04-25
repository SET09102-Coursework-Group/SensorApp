using System.Net;
using System.Text;
using System.Text.Json;
using SensorApp.Shared.Dtos.Incident;
using SensorApp.Shared.Services;

namespace SensorApp.Tests.UnitTests.Services;

public class IncidentApiServiceTests
{
    [Fact]
    public async Task GetAllIncidentsAsync_ReturnsListOfIncidents_WhenSuccessful()
    {
        // Arrange
        var mockIncidents = new List<IncidentDto>
        {
            new IncidentDto
            {
                Id = 1,
                Type = "Max threshold breach",
                Status = "Open",
                Sensor_id = 1,
                Creation_date = DateTime.UtcNow,
                Priority = "High",
                Comments = "Test incident"
            }
        };

        var responseContent = new StringContent(
            JsonSerializer.Serialize(mockIncidents),
            Encoding.UTF8,
            "application/json"
        );

        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = responseContent
        };

        var httpClient = HttpClientTestFactory.Create(response);
        var apiService = new IncidentApiService(httpClient);

        // Act
        var result = await apiService.GetAllIncidentsAsync("validToken");

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Max threshold breach", result[0].Type);
    }

    [Fact]
    public async Task GetAllIncidentsAsync_ReturnsEmptyList_WhenUnauthorized_Following401Response()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);

        var httpClient = HttpClientTestFactory.Create(response);
        var apiService = new IncidentApiService(httpClient);

        // Act
        var result = await apiService.GetAllIncidentsAsync("unauthorizedToken");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllIncidentsAsync_ReturnsEmptyList_OnError()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

        var httpClient = HttpClientTestFactory.Create(response);
        var apiService = new IncidentApiService(httpClient);

        // Act
        var result = await apiService.GetAllIncidentsAsync("badToken");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task AddIncidentAsync_ReturnsTrue_WhenIncidentAddedSuccessfully()
    {
        // Arrange
        var newIncident = new CreateIncidentDto
        {
            Type = "Max threshold breach",
            Status = "Open",
            SensorId = 1,
            Priority = "High",
            Comments = "New test incident"
        };

        var response = new HttpResponseMessage(HttpStatusCode.OK);

        var httpClient = HttpClientTestFactory.Create(response);
        var apiService = new IncidentApiService(httpClient);

        // Act
        var result = await apiService.AddIncidentAsync("validToken", newIncident);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task AddIncidentAsync_ReturnsFalse_WhenFailedToAddIncident()
    {
        // Arrange
        var newIncident = new CreateIncidentDto
        {
            Type = "Max threshold breach",
            Status = "Open",
            SensorId = 1,
            Priority = "High",
            Comments = "New test incident"
        };

        var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

        var httpClient = HttpClientTestFactory.Create(response);
        var apiService = new IncidentApiService(httpClient);

        // Act
        var result = await apiService.AddIncidentAsync("badToken", newIncident);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ResolveIncidentAsync_ReturnsTrue_WhenIncidentResolvedSuccessfully()
    {
        // Arrange
        var resolutionDto = new IncidentResolutionDto
        {
            ResolutionComments = "Incident resolved successfully."
        };

        var response = new HttpResponseMessage(HttpStatusCode.OK);

        var httpClient = HttpClientTestFactory.Create(response);
        var apiService = new IncidentApiService(httpClient);

        // Act
        var result = await apiService.ResolveIncidentAsync("validToken", 1, resolutionDto);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ResolveIncidentAsync_ReturnsFalse_WhenFailedToResolveIncident()
    {
        // Arrange
        var resolutionDto = new IncidentResolutionDto
        {
            ResolutionComments = "Incident resolution failed."
        };

        var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

        var httpClient = HttpClientTestFactory.Create(response);
        var apiService = new IncidentApiService(httpClient);

        // Act
        var result = await apiService.ResolveIncidentAsync("badToken", 1, resolutionDto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteIncidentAsync_ReturnsTrue_WhenIncidentDeletedSuccessfully()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        var httpClient = HttpClientTestFactory.Create(response);
        var apiService = new IncidentApiService(httpClient);

        // Act
        var result = await apiService.DeleteIncidentAsync("validToken", 1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteIncidentAsync_ReturnsFalse_WhenFailedToDeleteIncident()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

        var httpClient = HttpClientTestFactory.Create(response);
        var apiService = new IncidentApiService(httpClient);

        // Act
        var result = await apiService.DeleteIncidentAsync("badToken", 1);

        // Assert
        Assert.False(result);
    }
}
