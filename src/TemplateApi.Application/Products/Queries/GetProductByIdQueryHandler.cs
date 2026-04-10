using AutoMapper;
using MediatR;
using TemplateApi.Application.Products.DTOs;
using TemplateApi.Domain.Common.Interfaces;

namespace TemplateApi.Application.Products.Queries;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(
        IProductRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id, cancellationToken)
                      ?? throw new KeyNotFoundException($"Product {request.Id} não encontrado.");

        return _mapper.Map<ProductDto>(product);
    }
}