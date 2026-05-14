using MediatR;
using TemplateApi.Application.Auth.DTOs;

namespace TemplateApi.Application.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;
