using FluentAssertions;
using NSubstitute;
using TemplateApi.Application.Products.Commands.CreateProduct;
using TemplateApi.Domain.Common.Interfaces;
using TemplateApi.Domain.Entities;

namespace TemplateApi.Tests.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandlerTests
{
    private readonly IProductRepository _repository;
    private readonly CreateProductCommandHandler _handler;

    public CreateProductCommandHandlerTests()
    {
        _repository = Substitute.For<IProductRepository>();
        _handler = new CreateProductCommandHandler(_repository);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateProductAndReturnId()
    {
        // Arrange
        var command = new CreateProductCommand("Product", "Description", 100m);
        var ct = CancellationToken.None;

        // Act
        var result = await _handler.Handle(command, ct);

        // Assert
        result.Should().NotBeEmpty();
        await _repository.Received(1).AddAsync(Arg.Is<Product>(p => 
            p.Name == command.Name && 
            p.Price == command.Price), ct);
    }
}
