using MediatR;
using TemplateApi.Application.Auth.DTOs;
using TemplateApi.Domain.Common.Interfaces;

namespace TemplateApi.Application.Auth.Commands.Login;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IJwtService jwtService) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user == null || user.Pass != request.Password || !user.Active)
        {
            throw new UnauthorizedAccessException("Credenciais inválidas.");
        }

        var token = jwtService.GenerateToken(user);

        return new LoginResponse(token, user.Name, user.Email);
    }
}
