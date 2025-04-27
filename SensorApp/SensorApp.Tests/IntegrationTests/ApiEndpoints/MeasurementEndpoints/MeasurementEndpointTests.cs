using FluentAssertions;
using SensorApp.Shared.Dtos;
using SensorApp.Tests.IntegrationTests.ApiEndpoints.Helpers;
using System.Net;
using System.Net.Http.Json;

namespace SensorApp.Tests.IntegrationTests.ApiEndpoints.MeasurementEndpoints;

/// <summary>
/// Integration tests for the /measurements API endpoint.
/// </summary>
public class MeasurementEndpointTests : IClassFixture<WebApplicationFactoryForTests>
{
    private readonly HttpClient _client;
    private readonly TokenProvider _tokenProvider;

    public MeasurementEndpointTests(WebApplicationFactoryForTests factory)
    {
        _client = factory.CreateClient();
        _tokenProvider = new TokenProvider(_client);
    }

    [Fact]
    public async Task GetMeasurements_WithValidAdminToken_ReturnsOk()
    {
        // Arrange
        var token = await _tokenProvider.GetAdminTokenAsync();
        var request = TestHelpers.CreateAuthorizedRequest("/measurements", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var data = await response.Content.ReadFromJsonAsync<List<MeasurementDto>>();
        data.Should().NotBeNull();
    }

    [Fact]
    public async Task GetMeasurements_WithInvalidSensorId_ReturnsNotFound()
    {
        var token = await _tokenProvider.GetAdminTokenAsync();
        var request = TestHelpers.CreateAuthorizedRequest("/measurements?sensorId=111", token);

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetMeasurements_WithInvalidMeasurementTypeId_ReturnsNotFound()
    {
        var token = await _tokenProvider.GetAdminTokenAsync();
        var request = TestHelpers.CreateAuthorizedRequest("/measurements?measurementTypeId=1234", token);

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetMeasurements_WithUnauthorizedRole_ReturnsForbidden()
    {
        // Arrange
        var token = await _tokenProvider.GetOpsTokenAsync();
        var request = TestHelpers.CreateAuthorizedRequest("/measurements", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetMeasurements_WithoutToken_ReturnsUnauthorized()
    {
        var response = await _client.GetAsync("/measurements");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
