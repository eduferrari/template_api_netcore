using MediatR;

namespace TemplateApi.Application.Users.Commands.CreateUser;

public record CreateUserCommand(
    string Name,
    string Email,
    string Pass) : IRequest<Guid>;
