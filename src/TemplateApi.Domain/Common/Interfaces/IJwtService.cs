using TemplateApi.Domain.Entities;

namespace TemplateApi.Domain.Common.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}
