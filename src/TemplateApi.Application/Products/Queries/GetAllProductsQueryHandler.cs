using AutoMapper;
using MediatR;
using TemplateApi.Application.Products.DTOs;
using TemplateApi.Application.Products.Queries;
using TemplateApi.Domain.Common.Interfaces;

namespace TemplateApi.Application.Products.Queries;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public GetAllProductsQueryHandler(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<ProductDto>> Handle(
        GetAllProductsQuery request,
        CancellationToken cancellationToken)
    {
        var products = await _repository.GetAllAsync(
            request.Page, request.PageSize, cancellationToken);

        return _mapper.Map<List<ProductDto>>(products);
    }
}