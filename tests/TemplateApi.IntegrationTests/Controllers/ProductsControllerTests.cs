using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using TemplateApi.Application.Products.Commands.CreateProduct;
using TemplateApi.Application.Products.Commands.UpdateProduct;
using TemplateApi.IntegrationTests.Common;

namespace TemplateApi.IntegrationTests.Controllers;

public class ProductsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public ProductsControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        
        // Configura o client para usar o esquema de autenticação de teste
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
    }

    [Fact]
    public async Task Create_ShouldReturnCreated_WhenValidData()
    {
        // Arrange
        var command = new CreateProductCommand("Integracao Test", "Descricao", 99.99m);

        // Act
        var response = await _client.PostAsJsonAsync("/api/products", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await response.Content.ReadFromJsonAsync<CreatedResponse>();
        created!.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        // Act
        var response = await _client.GetAsync("/api/products");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Update_ShouldReturnNoContent_WhenProductExists()
    {
        // Arrange - Primeiro cria um produto
        var createCommand = new CreateProductCommand("Para Atualizar", "Desc", 10m);
        var createResponse = await _client.PostAsJsonAsync("/api/products", createCommand);
        var created = await createResponse.Content.ReadFromJsonAsync<CreatedResponse>();
        var productId = created!.Id;

        var updateCommand = new UpdateProductCommand(productId, "Atualizado", "Nova Desc", 15m);

        // Act
        var response = await _client.PutAsJsonAsync($"/api/products/{productId}", updateCommand);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenProductExists()
    {
        // Arrange
        var createCommand = new CreateProductCommand("Para Deletar", "Desc", 10m);
        var createResponse = await _client.PostAsJsonAsync("/api/products", createCommand);
        var created = await createResponse.Content.ReadFromJsonAsync<CreatedResponse>();
        var productId = created!.Id;

        // Act
        var response = await _client.DeleteAsync($"/api/products/{productId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    private record CreatedResponse(Guid Id);
}
