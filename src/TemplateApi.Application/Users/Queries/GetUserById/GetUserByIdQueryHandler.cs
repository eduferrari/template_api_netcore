using MediatR;
using TemplateApi.Application.Users.DTOs;
using TemplateApi.Domain.Common.Interfaces;

namespace TemplateApi.Application.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler(IUserRepository userRepository) 
    : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        var user = await userRepository.GetByIdAsync(request.Id, ct);
        
        if (user is null) return null;

        return new UserDto(user.Id, user.Name, user.Email, user.Active);
    }
}
