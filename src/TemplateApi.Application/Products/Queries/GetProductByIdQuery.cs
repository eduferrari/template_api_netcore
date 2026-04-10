using MediatR;
using TemplateApi.Application.Products.DTOs;

namespace TemplateApi.Application.Products.Queries;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto>;