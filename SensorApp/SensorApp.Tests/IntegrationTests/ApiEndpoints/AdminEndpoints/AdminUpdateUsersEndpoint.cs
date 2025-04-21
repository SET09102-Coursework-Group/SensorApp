using FluentAssertions;
using SensorApp.Shared.Dtos.Admin;
using SensorApp.Shared.Enums;
using SensorApp.Tests.IntegrationTests.ApiEndpoints.Helpers;
using System.Net;
using System.Net.Http.Json;

namespace SensorApp.Tests.IntegrationTests.ApiEndpoints.AdminEndpoints;

public class AdminUpdateUserEndpointTests(WebApplicationFactoryForTests factory) : IClassFixture<WebApplicationFactoryForTests>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly TokenProvider _tokenProvider = new(factory.CreateClient());
    private string _adminEmail = "admin@sensor.com";

    [Fact]
    public async Task Admin_Can_Update_Another_User()
    {
        // Arrange
        var token = await _tokenProvider.GetAdminTokenAsync();

        var userListRequest = TestHelpers.CreateAuthorizedRequest("/admin/users", token, HttpMethod.Get);
        var listResponse = await _client.SendAsync(userListRequest);
        listResponse.EnsureSuccessStatusCode();

        var allUsers = await listResponse.Content.ReadFromJsonAsync<List<UserWithRoleDto>>();
        var target = allUsers!.First(u => u.Email != _adminEmail);

        var newUsername = target.Username + "updatedTest";

        var updateDto = new UpdateUserDto
        {
            Username = newUsername,
            Email = target.Email,             
            Role = Enum.Parse<UserRole>(target.Role), 
            Password = null                  
        };

        var updateRequest = TestHelpers.CreateAuthorizedRequest($"/admin/users/{target.Id}", token, HttpMethod.Put, updateDto
        );

        // Act
        var updateResponse = await _client.SendAsync(updateRequest);

        // Assert 
        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getUpdatedUserRequest = TestHelpers.CreateAuthorizedRequest($"/admin/users/{target.Id}", token, HttpMethod.Get);
        var getUpdatedUserResponse = await _client.SendAsync(getUpdatedUserRequest);
        getUpdatedUserResponse.EnsureSuccessStatusCode();

        var updatedUser = await getUpdatedUserResponse.Content.ReadFromJsonAsync<UserWithRoleDto>();
        updatedUser.Should().NotBeNull();
        updatedUser!.Username.Should().Be(newUsername);
        updatedUser.Email.Should().Be(target.Email);
    }


    [Fact]
    public async Task Admin_Cannot_Update_Their_Own_Account()
    {
        var token = await _tokenProvider.GetAdminTokenAsync();

        var userListRequest = TestHelpers.CreateAuthorizedRequest("/admin/users", token, HttpMethod.Get);
        var listResponse = await _client.SendAsync(userListRequest);
        listResponse.EnsureSuccessStatusCode();

        var allUsers = await listResponse.Content.ReadFromJsonAsync<List<UserWithRoleDto>>();
        var adminUser = allUsers!.First(u => u.Email == _adminEmail);

        var dto = new UpdateUserDto
        {
            Username = adminUser.Username,
            Email = adminUser.Email,
            Role = Enum.Parse<UserRole>(adminUser.Role)
        };

        var request = TestHelpers.CreateAuthorizedRequest($"/admin/users/{adminUser.Id}", token, HttpMethod.Put, dto);

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("cannot update your own account");
    }


    [Fact]
    public async Task Cannot_Update_If_Username_Already_Exists()
    {
        var token = await _tokenProvider.GetAdminTokenAsync();

        var userListRequest = TestHelpers.CreateAuthorizedRequest("/admin/users", token, HttpMethod.Get);
        var listResponse = await _client.SendAsync(userListRequest);
        listResponse.EnsureSuccessStatusCode();

        var allUsers = await listResponse.Content.ReadFromJsonAsync<List<UserWithRoleDto>>();

        var target = allUsers!.First(u => u.Email != _adminEmail);
        var conflictUser = allUsers.First(u => u.Id != target.Id && u.Email != _adminEmail);

        var dto = new UpdateUserDto
        {
            Username = conflictUser.Username, 
            Email = "something_new@test.com",
            Role = Enum.Parse<UserRole>(target.Role)
        };

        var request = TestHelpers.CreateAuthorizedRequest($"/admin/users/{target.Id}", token, HttpMethod.Put, dto);

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Username is already in use");
    }


    [Fact]
    public async Task UnauthenticatedUser_IsUnauthorized()
    {
        var dto = new UpdateUserDto
        {
            Username = "newName",
            Email = "new@email.com",
            Role = UserRole.EnvironmentalScientist
        };

        var request = new HttpRequestMessage(HttpMethod.Put, $"/admin/users/{Guid.NewGuid()}")
        {
            Content = JsonContent.Create(dto)
        };

        var response = await _client.SendAsync(request);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}