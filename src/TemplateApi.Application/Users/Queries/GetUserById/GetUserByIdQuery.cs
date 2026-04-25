using MediatR;
using TemplateApi.Application.Users.DTOs;

namespace TemplateApi.Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid Id) : IRequest<UserDto?>;
