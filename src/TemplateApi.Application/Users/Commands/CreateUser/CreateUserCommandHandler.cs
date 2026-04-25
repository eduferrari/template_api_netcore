using MediatR;
using TemplateApi.Domain.Common.Interfaces;
using TemplateApi.Domain.Entities;

namespace TemplateApi.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _repository;

    public CreateUserCommandHandler(IUserRepository repository)
        => _repository = repository;

    public async Task<Guid> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var existingUser = await _repository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Email already in use.");
        }

        var user = User.Create(
            request.Name,
            request.Email,
            request.Pass);

        await _repository.AddAsync(user, cancellationToken);

        return user.Id;
    }
}
