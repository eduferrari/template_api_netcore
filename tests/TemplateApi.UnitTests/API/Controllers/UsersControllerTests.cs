using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using TemplateApi.API.Controllers;
using TemplateApi.Application.Users.Commands.CreateUser;
using TemplateApi.Application.Users.Commands.InactivateUser;
using TemplateApi.Application.Users.Commands.UpdateUser;
using TemplateApi.Application.Users.DTOs;
using TemplateApi.Application.Users.Queries.GetAllUsers;
using TemplateApi.Application.Users.Queries.GetUserById;

namespace TemplateApi.UnitTests.API.Controllers;

public class UsersControllerTests
{
    private readonly IMediator _mediator;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _controller = new UsersController(_mediator);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var command = new CreateUserCommand("Test", "test@test.com", "pass");
        var userId = Guid.NewGuid();
        _mediator.Send(command, Arg.Any<CancellationToken>())
            .Returns(userId);

        // Act
        var result = await _controller.Create(command, default);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.ActionName.Should().Be("GetById");
        createdResult.RouteValues!["id"].Should().Be(userId);
    }

    [Fact]
    public async Task Update_ShouldReturnNoContent()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new UpdateUserCommand(userId, "New Name", "newpass");

        // Act
        var result = await _controller.Update(userId, command, default);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        await _mediator.Received(1).Send(Arg.Is<UpdateUserCommand>(c => c.Id == userId), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Inactivate_ShouldReturnNoContent()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var result = await _controller.Inactivate(userId, default);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        await _mediator.Received(1).Send(Arg.Is<InactivateUserCommand>(c => c.Id == userId), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userDto = new UserDto(userId, "Test User", "test@test.com", true);
        _mediator.Send(Arg.Any<GetUserByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(userDto);

        // Act
        var result = await _controller.GetById(userId, default);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(userDto);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WithUsers()
    {
        // Arrange
        var users = new List<UserDto>
        {
            new(Guid.NewGuid(), "User 1", "user1@test.com", true),
            new(Guid.NewGuid(), "User 2", "user2@test.com", true)
        };
        _mediator.Send(Arg.Any<GetAllUsersQuery>(), Arg.Any<CancellationToken>())
            .Returns(users);

        // Act
        var result = await _controller.GetAll(default);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(users);
    }
}
