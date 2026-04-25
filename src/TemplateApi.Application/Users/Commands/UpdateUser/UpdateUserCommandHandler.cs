using MediatR;
using TemplateApi.Domain.Common.Interfaces;

namespace TemplateApi.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IUserRepository _repository;

    public UpdateUserCommandHandler(IUserRepository repository)
        => _repository = repository;

    public async Task Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        user.Update(request.Name, request.Pass);

        await _repository.UpdateAsync(user, cancellationToken);
    }
}
