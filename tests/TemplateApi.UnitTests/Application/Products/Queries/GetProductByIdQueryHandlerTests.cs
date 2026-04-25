using AutoMapper;
using FluentAssertions;
using NSubstitute;
using TemplateApi.Application.Products.DTOs;
using TemplateApi.Application.Products.Queries;
using TemplateApi.Domain.Common.Interfaces;
using TemplateApi.Domain.Entities;

namespace TemplateApi.UnitTests.Application.Products.Queries;

public class GetProductByIdQueryHandlerTests
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    private readonly GetProductByIdQueryHandler _handler;

    public GetProductByIdQueryHandlerTests()
    {
        _repository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetProductByIdQueryHandler(_repository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnProductDto_WhenProductExists()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = Product.Create("Test", "Desc", 10m);
        var dto = new ProductDto(productId, "Test", "Desc", 10m, true);

        _repository.GetByIdAsync(productId, Arg.Any<CancellationToken>())
            .Returns(product);
        _mapper.Map<ProductDto>(product).Returns(dto);

        var query = new GetProductByIdQuery(productId);

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task Handle_ShouldThrowKeyNotFoundException_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _repository.GetByIdAsync(productId, Arg.Any<CancellationToken>())
            .Returns((Product?)null);

        var query = new GetProductByIdQuery(productId);

        // Act
        var act = () => _handler.Handle(query, default);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Product {productId} não encontrado.");
    }
}
