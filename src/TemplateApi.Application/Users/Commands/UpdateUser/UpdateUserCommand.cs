using MediatR;

namespace TemplateApi.Application.Users.Commands.UpdateUser;

public record UpdateUserCommand(
    Guid Id,
    string Name,
    string Pass) : IRequest;
