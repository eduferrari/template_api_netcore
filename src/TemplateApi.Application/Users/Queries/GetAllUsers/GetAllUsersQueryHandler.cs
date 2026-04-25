using MediatR;
using TemplateApi.Application.Users.DTOs;
using TemplateApi.Domain.Common.Interfaces;

namespace TemplateApi.Application.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler(IUserRepository userRepository) 
    : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
{
    public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken ct)
    {
        var users = await userRepository.GetAllAsync(ct);
        
        return users.Select(u => new UserDto(u.Id, u.Name, u.Email, u.Active));
    }
}
