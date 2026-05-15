using MediatR;

namespace TemplateApi.Application.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    bool? IsActive = null) : IRequest<Guid>;