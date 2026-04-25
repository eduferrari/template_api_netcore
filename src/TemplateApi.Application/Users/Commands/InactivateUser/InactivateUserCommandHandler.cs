using MediatR;
using TemplateApi.Domain.Common.Interfaces;

namespace TemplateApi.Application.Users.Commands.InactivateUser;

public class InactivateUserCommandHandler : IRequestHandler<InactivateUserCommand>
{
    private readonly IUserRepository _repository;

    public InactivateUserCommandHandler(IUserRepository repository)
        => _repository = repository;

    public async Task Handle(
        InactivateUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        user.Deactivate();

        await _repository.UpdateAsync(user, cancellationToken);
    }
}
