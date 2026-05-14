using MediatR;

namespace TemplateApi.Application.Users.Commands.InactivateUser;

public record InactivateUserCommand(Guid Id) : IRequest;
