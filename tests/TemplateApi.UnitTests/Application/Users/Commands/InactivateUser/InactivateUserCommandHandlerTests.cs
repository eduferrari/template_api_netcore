using FluentAssertions;
using NSubstitute;
using TemplateApi.Application.Users.Commands.InactivateUser;
using TemplateApi.Domain.Common;
using TemplateApi.Domain.Common.Interfaces;
using TemplateApi.Domain.Entities;

namespace TemplateApi.UnitTests.Application.Users.Commands.InactivateUser;

public class InactivateUserCommandHandlerTests
{
    private readonly IUserRepository _repository;
    private readonly InactivateUserCommandHandler _handler;

    public InactivateUserCommandHandlerTests()
    {
        _repository = Substitute.For<IUserRepository>();
        _handler = new InactivateUserCommandHandler(_repository);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldInactivateUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = User.Create("User", "test@test.com", "pass");
        typeof(BaseEntity).GetProperty("Id")?.SetValue(user, userId);

        var command = new InactivateUserCommand(userId);
        var ct = CancellationToken.None;
        _repository.GetByIdAsync(userId, ct).Returns(user);

        // Act
        await _handler.Handle(command, ct);

        // Assert
        user.Active.Should().BeFalse();
        await _repository.Received(1).UpdateAsync(user, ct);
    }

    [Fact]
    public async Task Handle_UserNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new InactivateUserCommand(userId);
        var ct = CancellationToken.None;
        _repository.GetByIdAsync(userId, ct).Returns((User?)null);

        // Act
        var act = () => _handler.Handle(command, ct);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("User not found.");
    }
}
