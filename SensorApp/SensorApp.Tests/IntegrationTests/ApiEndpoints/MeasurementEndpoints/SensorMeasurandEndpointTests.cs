using FluentAssertions;
using SensorApp.Shared.Dtos;
using SensorApp.Tests.IntegrationTests.ApiEndpoints.Helpers;
using System.Net;
using System.Net.Http.Json;

namespace SensorApp.Tests.IntegrationTests.ApiEndpoints.MeasurementEndpoints;

public class SensorMeasurandEndpointTests: IClassFixture<WebApplicationFactoryForTests>
{
    private readonly HttpClient _client;
    private readonly TokenProvider _tokenProvider;
    private readonly WebApplicationFactoryForTests _factory;

    public SensorMeasurandEndpointTests(WebApplicationFactoryForTests factory)
    {

        _factory = factory;
        _client = factory.CreateClient(); 
        _tokenProvider = new TokenProvider(_client);

    }

    [Fact]
    public async Task GetSensorMeasurands_WithValidAdminToken_ReturnsOk()
    {
        // Arrange
        var token = await _tokenProvider.GetAdminTokenAsync();
        var request = TestHelpers.CreateAuthorizedRequest("/sensors/1/measurands", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var data = await response.Content.ReadFromJsonAsync<List<MeasurandDto>>();
        data.Should().NotBeNull();
    }

    [Fact]
    public async Task GetSensorMeasurands_WithoutToken_ReturnsUnauthorized()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/sensors/1/measurands");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetSensorMeasurands_WithOpsRole_ReturnsForbidden()
    {
        // Arrange
        var token = await _tokenProvider.GetOpsTokenAsync();
        var request = TestHelpers.CreateAuthorizedRequest("/sensors/1/measurands", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetSensorMeasurands_WithNonExistingSensorId_ReturnsOkWithEmptyList()
    {
        // Arrange
        var token = await _tokenProvider.GetAdminTokenAsync();
        var request = TestHelpers.CreateAuthorizedRequest("/sensors/12345/measurands", token); 

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var data = await response.Content.ReadFromJsonAsync<List<MeasurandDto>>();
        data.Should().NotBeNull();
        data.Should().BeEmpty();
    }

}