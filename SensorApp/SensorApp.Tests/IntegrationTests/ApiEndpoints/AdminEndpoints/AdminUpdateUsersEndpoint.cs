using FluentAssertions;
using SensorApp.Shared.Dtos.Admin;
using SensorApp.Shared.Enums;
using SensorApp.Tests.IntegrationTests.ApiEndpoints.AuthEndpoints;
using SensorApp.Tests.IntegrationTests.ApiEndpoints.Helpers;
using System.Net;
using System.Net.Http.Json;

namespace SensorApp.Tests.IntegrationTests.ApiEndpoints.AdminEndpoints;

public class AdminUpdateUsersEndpoint(WebApplicationFactoryForTests factory) : IClassFixture<WebApplicationFactoryForTests>
{
    private readonly HttpClient _client = factory.CreateClient();

    //[Fact]
    //public async Task Admin_UpdatesUserRole_Successfully()
    //{
    //    // Arrange
    //    var token = await new TestUserBuilder(_client)
    //        .WithCredentials(TestUsers.AdminEmail, TestUsers.AdminPassword)
    //        .BuildTokenAsync();

    //    var email = $"rolechange_{Guid.NewGuid()}@sensor.com";

    //    var newUser = new CreateUserDto
    //    {
    //        Username = email,
    //        Email = email,
    //        Password = "RoleChanged123!",
    //        Role = UserRole.OperationsManager
    //    };

    //    var createRequest = TestHelpers.CreateAuthorizedRequest("/admin/users", token, HttpMethod.Post, newUser);
    //    var createResponse = await _client.SendAsync(createRequest);
    //    createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

    //    var createdUser = await createResponse.Content.ReadFromJsonAsync<UserWithRoleDto>();
    //    createdUser.Should().NotBeNull();

    //    // Act 
    //    var changeRoleRequest = TestHelpers.CreateAuthorizedRequest(
    //        $"/admin/users/{createdUser!.Id}/role?role={UserRole.Administrator}",
    //        token,
    //        HttpMethod.Put
    //    );

    //    var changeResponse = await _client.SendAsync(changeRoleRequest);

    //    // Assert
    //    changeResponse.StatusCode.Should().Be(HttpStatusCode.OK);

    //    var updatedUser = await GetUserByIdAsync(createdUser.Id, token);
    //    updatedUser.Should().NotBeNull();
    //    updatedUser!.Role.Should().Be(UserRole.Administrator.ToString());
    //}

    //[Fact]
    //public async Task Admin_CannotChangeOwnRole_ReturnsBadRequest()
    //{
    //    // Arrange
    //    var token = await new TestUserBuilder(_client)
    //        .WithCredentials(TestUsers.AdminEmail, TestUsers.AdminPassword)
    //        .BuildTokenAsync();

    //    var getRequest = TestHelpers.CreateAuthorizedRequest("/admin/users", token, HttpMethod.Get);
    //    var getResponse = await _client.SendAsync(getRequest);
    //    var users = await getResponse.Content.ReadFromJsonAsync<List<UserWithRoleDto>>();

    //    var currentUser = users!.First(u => u.Email == TestUsers.AdminEmail);
    //    var originalRole = currentUser.Role;

    //    // Act
    //    var updateRequest = TestHelpers.CreateAuthorizedRequest(
    //        $"/admin/users/{currentUser.Id}/role?role={UserRole.OperationsManager}",
    //        token,
    //        HttpMethod.Put
    //    );

    //    var updateResponse = await _client.SendAsync(updateRequest);

    //    // Assert
    //    updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

    //    var updatedUser = await GetUserByIdAsync(currentUser.Id, token);
    //    updatedUser.Should().NotBeNull();
    //    updatedUser!.Role.Should().Be(originalRole, "User should not be able to change their own role");
    //}

    //private async Task<UserWithRoleDto?> GetUserByIdAsync(string userId, string token)
    //{
    //    var request = TestHelpers.CreateAuthorizedRequest("/admin/users", token, HttpMethod.Get);
    //    var response = await _client.SendAsync(request);
    //    response.EnsureSuccessStatusCode();

    //    var users = await response.Content.ReadFromJsonAsync<List<UserWithRoleDto>>();
    //    return users!.FirstOrDefault(u => u.Id == userId);
    //}
}