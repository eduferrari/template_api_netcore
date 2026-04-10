using FluentAssertions;
using NSubstitute;
using TemplateApi.Application.Products.Commands.UpdateProduct;
using TemplateApi.Domain.Common.Interfaces;
using TemplateApi.Domain.Entities;

namespace TemplateApi.UnitTests.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandlerTests
{
    private readonly IProductRepository _repository;
    private readonly UpdateProductCommandHandler _handler;

    public UpdateProductCommandHandlerTests()
    {
        _repository = Substitute.For<IProductRepository>();
        _handler = new UpdateProductCommandHandler(_repository);
    }

    [Fact]
    public async Task Handle_ProductExists_ShouldUpdateProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var existingProduct = Product.Create("Old Name", "Old Desc", 10m);
        // Usando reflexão para setar o ID se necessário, mas como Product é uma classe comum, 
        // e BaseEntity deve ter Id Guid, e Product.Create gera um novo Guid.
        // Se eu precisar de um Guid específico, posso precisar ajustar o BaseEntity ou usar reflexão.
        // Mas por enquanto vamos assumir que GetByIdAsync retorna o que eu configurar.
        
        var command = new UpdateProductCommand(productId, "New Name", "New Desc", 20m);
        var ct = CancellationToken.None;

        _repository.GetByIdAsync(productId, ct).Returns(existingProduct);

        // Act
        await _handler.Handle(command, ct);

        // Assert
        existingProduct.Name.Should().Be(command.Name);
        existingProduct.Price.Should().Be(command.Price);
        await _repository.Received(1).UpdateAsync(existingProduct, ct);
    }

    [Fact]
    public async Task Handle_ProductDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new UpdateProductCommand(productId, "Name", "Desc", 10m);
        var ct = CancellationToken.None;

        _repository.GetByIdAsync(productId, ct).Returns((Product?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, ct);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
