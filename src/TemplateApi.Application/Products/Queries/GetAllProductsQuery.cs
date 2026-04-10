using MediatR;
using TemplateApi.Application.Products.DTOs;

namespace TemplateApi.Application.Products.Queries;

public record GetAllProductsQuery(int Page = 1, int PageSize = 20) : IRequest<List<ProductDto>>;