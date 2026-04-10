using FluentAssertions;
using NSubstitute;
using TemplateApi.Application.Products.Commands.DeleteProduct;
using TemplateApi.Domain.Common.Interfaces;
using TemplateApi.Domain.Entities;

namespace TemplateApi.UnitTests.Application.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandlerTests
{
    private readonly IProductRepository _repository;
    private readonly DeleteProductCommandHandler _handler;

    public DeleteProductCommandHandlerTests()
    {
        _repository = Substitute.For<IProductRepository>();
        _handler = new DeleteProductCommandHandler(_repository);
    }

    [Fact]
    public async Task Handle_ProductExists_ShouldDeleteProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var existingProduct = Product.Create("Product", "Desc", 10m);
        var ct = CancellationToken.None;

        _repository.GetByIdAsync(productId, ct).Returns(existingProduct);

        // Act
        await _handler.Handle(new DeleteProductCommand(productId), ct);

        // Assert
        await _repository.Received(1).DeleteAsync(productId, ct);
    }

    [Fact]
    public async Task Handle_ProductDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var ct = CancellationToken.None;

        _repository.GetByIdAsync(productId, ct).Returns((Product?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(new DeleteProductCommand(productId), ct);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
