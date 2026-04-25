using AutoMapper;
using FluentAssertions;
using NSubstitute;
using TemplateApi.Application.Mappings;
using TemplateApi.Application.Products.DTOs;
using TemplateApi.Application.Products.Queries;
using TemplateApi.Domain.Common.Interfaces;
using TemplateApi.Domain.Entities;

namespace TemplateApi.UnitTests.Application.Products.Queries;

public class GetAllProductsQueryHandlerTests
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    private readonly GetAllProductsQueryHandler _handler;

    public GetAllProductsQueryHandlerTests()
    {
        _repository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetAllProductsQueryHandler(_repository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfProductDtos()
    {
        // Arrange
        var products = new List<Product>
        {
            Product.Create("P1", "D1", 10m),
            Product.Create("P2", "D2", 20m)
        };
        var dtos = new List<ProductDto>
        {
            new(Guid.NewGuid(), "P1", "D1", 10m, true),
            new(Guid.NewGuid(), "P2", "D2", 20m, true)
        };

        _repository.GetAllAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(products);
        _mapper.Map<List<ProductDto>>(products).Returns(dtos);

        var query = new GetAllProductsQuery(1, 10);

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.Should().BeEquivalentTo(dtos);
    }
}
