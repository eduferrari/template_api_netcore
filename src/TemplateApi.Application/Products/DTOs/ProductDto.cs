namespace TemplateApi.Application.Products.DTOs;

public record ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    bool IsActive);