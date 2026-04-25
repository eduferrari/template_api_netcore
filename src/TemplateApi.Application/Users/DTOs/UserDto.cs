namespace TemplateApi.Application.Users.DTOs;

public record UserDto(
    Guid Id,
    string Name,
    string Email,
    bool Active);
