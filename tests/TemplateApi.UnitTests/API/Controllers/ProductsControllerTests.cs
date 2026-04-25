using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using TemplateApi.API.Controllers;
using TemplateApi.Application.Products.Commands.CreateProduct;
using TemplateApi.Application.Products.Commands.DeleteProduct;
using TemplateApi.Application.Products.Commands.UpdateProduct;
using TemplateApi.Application.Products.DTOs;
using TemplateApi.Application.Products.Queries;

namespace TemplateApi.UnitTests.API.Controllers;

public class ProductsControllerTests
{
    private readonly IMediator _mediator;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _controller = new ProductsController(_mediator);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WithProducts()
    {
        // Arrange
        var products = new List<ProductDto> { new(Guid.NewGuid(), "Test", "Desc", 10m, true) };
        _mediator.Send(Arg.Any<GetAllProductsQuery>(), Arg.Any<CancellationToken>())
            .Returns(products);

    // Act
    var result = await _controller.GetAll();

    // Assert
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.Value.Should().BeEquivalentTo(products);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenProductExists()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new ProductDto(productId, "Test", "Desc", 10m, true);
        _mediator.Send(Arg.Any<GetProductByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(product);

        // Act
        var result = await _controller.GetById(productId, default);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(product);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var command = new CreateProductCommand("Test", "Desc", 10m);
        var productId = Guid.NewGuid();
        _mediator.Send(command, Arg.Any<CancellationToken>())
            .Returns(productId);

        // Act
        var result = await _controller.Create(command, default);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.ActionName.Should().Be(nameof(ProductsController.GetById));
        createdResult.RouteValues!["id"].Should().Be(productId);
    }

    [Fact]
    public async Task Update_ShouldReturnNoContent()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new UpdateProductCommand(productId, "Test", "Desc", 10m);

        // Act
        var result = await _controller.Update(productId, command, default);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        await _mediator.Received(1).Send(Arg.Is<UpdateProductCommand>(c => c.Id == productId), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent()
    {
        // Arrange
        var productId = Guid.NewGuid();

        // Act
        var result = await _controller.Delete(productId, default);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        await _mediator.Received(1).Send(Arg.Is<DeleteProductCommand>(c => c.Id == productId), Arg.Any<CancellationToken>());
    }
}
