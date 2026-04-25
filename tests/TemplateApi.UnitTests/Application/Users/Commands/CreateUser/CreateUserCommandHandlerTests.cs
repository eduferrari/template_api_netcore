using FluentAssertions;
using NSubstitute;
using TemplateApi.Application.Users.Commands.CreateUser;
using TemplateApi.Domain.Common.Interfaces;
using TemplateApi.Domain.Entities;

namespace TemplateApi.UnitTests.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandlerTests
{
    private readonly IUserRepository _repository;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _repository = Substitute.For<IUserRepository>();
        _handler = new CreateUserCommandHandler(_repository);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateUserAndReturnId()
    {
        // Arrange
        var command = new CreateUserCommand("User Test", "test@test.com", "password123");
        var ct = CancellationToken.None;
        _repository.GetByEmailAsync(command.Email, ct).Returns((User?)null);

        // Act
        var result = await _handler.Handle(command, ct);

        // Assert
        result.Should().NotBeEmpty();
        await _repository.Received(1).AddAsync(Arg.Is<User>(u => 
            u.Name == command.Name && 
            u.Email == command.Email), ct);
    }

    [Fact]
    public async Task Handle_DuplicateEmail_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var command = new CreateUserCommand("User Test", "test@test.com", "password123");
        var ct = CancellationToken.None;
        var existingUser = User.Create("Existing", "test@test.com", "pass");
        _repository.GetByEmailAsync(command.Email, ct).Returns(existingUser);

        // Act
        var act = () => _handler.Handle(command, ct);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Email already in use.");
    }
}
