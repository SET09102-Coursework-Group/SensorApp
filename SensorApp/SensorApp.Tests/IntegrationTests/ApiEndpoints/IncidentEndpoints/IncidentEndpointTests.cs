using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SensorApp.Shared.Dtos.Admin;
using SensorApp.Shared.Dtos.Incident;
using SensorApp.Tests.IntegrationTests.ApiEndpoints.Helpers;
using SensorApp.Shared.Enums;


namespace SensorApp.Tests.IntegrationTests
{
    public class IncidentEndpointsTests : IClassFixture<WebApplicationFactoryForTests>
    {
        private readonly HttpClient _client;
        private readonly TokenProvider _tokenProvider;

        public IncidentEndpointsTests(WebApplicationFactoryForTests factory)
        {
            _client = factory.CreateClient();
            _tokenProvider = new TokenProvider(_client);
        }

        [Fact]
        public async Task GetIncidents_ReturnsOkResponse_WithIncidents()
        {
            //Arrange
            var token = await _tokenProvider.GetOpsTokenAsync();
            var request = TestHelpers.CreateAuthorizedRequest("/incident", token);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var incidents = await response.Content.ReadFromJsonAsync<List<IncidentDto>>();
            Assert.NotNull(incidents);
        }

        [Fact]
        public async Task GetIncidents_ReturnsUnauthorized_WhenNotAuthenticated()
        {
            // Arrange
            var request = TestHelpers.CreateAuthorizedRequest("/incident", "");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task CreateIncident_ReturnsOkResponse_WithValidData()
        {
            // Arrange
            var newIncident = new CreateIncidentDto
            {
                Type = IncidentType.MaxThresholdBreached,
                Status = IncidentStatus.Open,
                SensorId = 1,
                Priority = IncidentPriority.High,
                Comments = "High concentration"
            };

            var token = await _tokenProvider.GetOpsTokenAsync();
            var request = TestHelpers.CreateAuthorizedRequest("/incident/create", token, HttpMethod.Post, newIncident);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var created = await response.Content.ReadFromJsonAsync<IncidentDto>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(newIncident.Type, created.Type);
            Assert.Equal(newIncident.Comments, created.Comments);
        }

        [Fact]
        public async Task CreateIncident_ReturnsBadRequest_WhenInvalidData()
        {
            // Arrange
            var invalidIncident = new CreateIncidentDto
            {
                Type = IncidentType.MaxThresholdBreached,
                Status = IncidentStatus.Open,
                Priority = IncidentPriority.High,
                Comments = "High concentration"
            };

            var token = await _tokenProvider.GetOpsTokenAsync();
            var request = TestHelpers.CreateAuthorizedRequest("/incident/create", token, HttpMethod.Post, invalidIncident);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateIncident_ReturnsUnauthorized_WhenNotAuthenticated()
        {
            // Arrange
            var newIncident = new CreateIncidentDto
            {
                Type = IncidentType.MaxThresholdBreached,
                Status = IncidentStatus.Open,
                SensorId = 1,
                Priority = IncidentPriority.High,
                Comments = "High concentration"
            };

            var request = TestHelpers.CreateAuthorizedRequest("/incident/create", "", HttpMethod.Post, newIncident);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ResolveIncident_ReturnsOkResponse_WhenResolvedSuccessfully()
        {
            // Arrange
            var incidentResolutionDto = new IncidentResolutionDto
            {
                ResolutionComments = "Resolved after verification"
            };

            var token = await _tokenProvider.GetOpsTokenAsync();
            var request = TestHelpers.CreateAuthorizedRequest("/incident/resolve/1", token, HttpMethod.Put, incidentResolutionDto);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode(); 
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ResolveIncident_ReturnsUnauthorized_WhenNotAuthenticated()
        {
            // Arrange
            var incidentResolutionDto = new IncidentResolutionDto
            {
                ResolutionComments = "Resolved after verification"
            };

            var request = TestHelpers.CreateAuthorizedRequest("/incident/resolve/1", "", HttpMethod.Put, incidentResolutionDto);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ResolveIncident_ReturnsNotFound_WhenIncidentDoesNotExist()
        {
            // Arrange
            var incidentResolutionDto = new IncidentResolutionDto
            {
                ResolutionComments = "Resolved after verification"
            };

            var token = await _tokenProvider.GetOpsTokenAsync();
            var request = TestHelpers.CreateAuthorizedRequest("/incident/resolve/999", token, HttpMethod.Put, incidentResolutionDto);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteIncident_ReturnsOkResponse_WhenDeletedSuccessfully()
        {
            //Arrange
            var token = await _tokenProvider.GetOpsTokenAsync();
            var request = TestHelpers.CreateAuthorizedRequest("/incident/delete/1", token, HttpMethod.Delete);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteIncident_ReturnsNotFound_WhenIncidentDoesNotExist()
        {
            //Arrange
            var token = await _tokenProvider.GetOpsTokenAsync();
            var request = TestHelpers.CreateAuthorizedRequest("/incident/delete/999", token, HttpMethod.Delete);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteIncident_ReturnsUnauthorized_WhenNotAuthenticated()
        {
            //Arrange
            var request = TestHelpers.CreateAuthorizedRequest("/incident/delete/1", "", HttpMethod.Delete);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
