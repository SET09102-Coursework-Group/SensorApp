using FluentAssertions;
using SensorApp.Shared.Dtos;
using SensorApp.Shared.Dtos.Admin;
using SensorApp.Shared.Enums;
using SensorApp.Tests.IntegrationTests.ApiEndpoints.Helpers;
using System.Net;
using System.Net.Http.Json;

namespace SensorApp.Tests.IntegrationTests.ApiEndpoints.AdminEndpoints;

/// <summary>
/// Integration tests for the admin endpoint responsible for updating user information.
/// </summary>
public class AdminUpdateUserEndpointTests : IClassFixture<WebApplicationFactoryForTests>
{
    private readonly HttpClient _client;
    private readonly TokenProvider _tokenProvider;
    private readonly string _adminEmail = "admin@sensor.com";

    public AdminUpdateUserEndpointTests(WebApplicationFactoryForTests factory)
    {
        _client = factory.CreateClient();
        _tokenProvider = new TokenProvider(factory.CreateClient());
    }

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

        var newUsername = target.Username + "_updated";
        var newEmail = "updated_" + target.Email;
        var newPassword = "Updated@ssword456!";
        var newRole = UserRole.OperationsManager;

        var updateDto = new UpdateUserDto
        {
            Username = newUsername,
            Email = newEmail,
            Role = newRole,
            Password = newPassword
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
        updatedUser.Email.Should().Be(newEmail);
        updatedUser.Role.Should().Be(newRole);

        var loginDto = new LoginDto(newUsername, newPassword);
        var loginResponse = await _client.PostAsJsonAsync("/login", loginDto);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var auth = await loginResponse.Content.ReadFromJsonAsync<AuthResponseDto>();
        auth.Should().NotBeNull();
        auth!.Username.Should().Be(newUsername);
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
            Role = adminUser.Role
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
            Role = target.Role
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

    [Fact]
    public async Task UpdateUser_DuplicateEmail_ReturnsConflict()
    {
        var adminToken = await _tokenProvider.GetAdminTokenAsync();
        var emailA = $"dupA_{Guid.NewGuid()}@sensor.com";
        var emailB = $"dupB_{Guid.NewGuid()}@sensor.com";

        async Task<string> Create(string mail)
        {
            var dto = new CreateUserDto { Username = mail, Email = mail, Password = "Password123!", Role = UserRole.EnvironmentalScientist };
            var req = TestHelpers.CreateAuthorizedRequest("/admin/users", adminToken, HttpMethod.Post, dto);
            var resp = await _client.SendAsync(req);
            var created = await resp.Content.ReadFromJsonAsync<UserWithRoleDto>();
            return created!.Id;
        }

        var idA = await Create(emailA);
        var idB = await Create(emailB);

        var updateDto = new UpdateUserDto { Username = emailA, Email = emailB, Role = UserRole.EnvironmentalScientist };
        var updReq = TestHelpers.CreateAuthorizedRequest($"/admin/users/{idA}", adminToken, HttpMethod.Put, updateDto);

        var updResp = await _client.SendAsync(updReq);

        updResp.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Admin_UpdateUser_WithUnchangedRole_IsNoContent()
    {
        var token = await _tokenProvider.GetAdminTokenAsync();

        var listReq = TestHelpers.CreateAuthorizedRequest("/admin/users", token, HttpMethod.Get);
        var listResp = await _client.SendAsync(listReq);
        listResp.EnsureSuccessStatusCode();

        var allUsers = await listResp.Content.ReadFromJsonAsync<List<UserWithRoleDto>>();
        var target = allUsers!.First(u => u.Email != _adminEmail);  

        var dto = new UpdateUserDto
        {
            Username = target.Username + "_update",
            Email = "_update" + target.Email,
            Role = target.Role               
        };

        var updReq = TestHelpers.CreateAuthorizedRequest($"/admin/users/{target.Id}", token, HttpMethod.Put, dto);
        var updResp = await _client.SendAsync(updReq);

        updResp.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }


}