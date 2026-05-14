using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TemplateApi.Application.Users.Commands.CreateUser;
using TemplateApi.Application.Users.Commands.UpdateUser;
using TemplateApi.IntegrationTests.Common;

namespace TemplateApi.IntegrationTests.Controllers;

public class UsersControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public UsersControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
    }

    [Fact]
    public async Task Create_ShouldReturnCreated_WhenValidData()
    {
        // Arrange
        var command = new CreateUserCommand("Integration User", "integration@test.com", "pass123");

        // Act
        var response = await _client.PostAsJsonAsync("/api/users", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await response.Content.ReadFromJsonAsync<CreatedResponse>();
        created!.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Update_ShouldReturnNoContent_WhenUserExists()
    {
        // Arrange
        var createCommand = new CreateUserCommand("To Update", "update@test.com", "pass");
        var createResponse = await _client.PostAsJsonAsync("/api/users", createCommand);
        var created = await createResponse.Content.ReadFromJsonAsync<CreatedResponse>();
        var userId = created!.Id;

        var updateCommand = new UpdateUserCommand(userId, "Updated Name", "newpass");

        // Act
        var response = await _client.PutAsJsonAsync($"/api/users/{userId}", updateCommand);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Inactivate_ShouldReturnNoContent_WhenUserExists()
    {
        // Arrange
        var createCommand = new CreateUserCommand("To Inactivate", "inactivate@test.com", "pass");
        var createResponse = await _client.PostAsJsonAsync("/api/users", createCommand);
        var created = await createResponse.Content.ReadFromJsonAsync<CreatedResponse>();
        var userId = created!.Id;

        // Act
        var response = await _client.PatchAsync($"/api/users/{userId}/inactivate", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk()
    {
        // Act
        var response = await _client.GetAsync($"/api/users/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private record CreatedResponse(Guid Id);
}
