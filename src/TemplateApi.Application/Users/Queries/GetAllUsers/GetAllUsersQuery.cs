using MediatR;
using TemplateApi.Application.Users.DTOs;

namespace TemplateApi.Application.Users.Queries.GetAllUsers;

public record GetAllUsersQuery : IRequest<IEnumerable<UserDto>>;
