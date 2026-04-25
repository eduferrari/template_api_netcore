using FluentAssertions;
using NSubstitute;
using TemplateApi.Application.Users.Commands.UpdateUser;
using TemplateApi.Domain.Common;
using TemplateApi.Domain.Common.Interfaces;
using TemplateApi.Domain.Entities;

namespace TemplateApi.UnitTests.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandlerTests
{
    private readonly IUserRepository _repository;
    private readonly UpdateUserCommandHandler _handler;

    public UpdateUserCommandHandlerTests()
    {
        _repository = Substitute.For<IUserRepository>();
        _handler = new UpdateUserCommandHandler(_repository);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldUpdateUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = User.Create("Old Name", "test@test.com", "oldpass");
        // Forçar o ID do usuário para o ID que estamos procurando (já que o Id é setado no BaseEntity)
        typeof(BaseEntity).GetProperty("Id")?.SetValue(user, userId);

        var command = new UpdateUserCommand(userId, "New Name", "newpass");
        var ct = CancellationToken.None;
        _repository.GetByIdAsync(userId, ct).Returns(user);

        // Act
        await _handler.Handle(command, ct);

        // Assert
        user.Name.Should().Be(command.Name);
        user.Pass.Should().Be(command.Pass);
        await _repository.Received(1).UpdateAsync(user, ct);
    }

    [Fact]
    public async Task Handle_UserNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new UpdateUserCommand(userId, "New Name", "newpass");
        var ct = CancellationToken.None;
        _repository.GetByIdAsync(userId, ct).Returns((User?)null);

        // Act
        var act = () => _handler.Handle(command, ct);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("User not found.");
    }
}
