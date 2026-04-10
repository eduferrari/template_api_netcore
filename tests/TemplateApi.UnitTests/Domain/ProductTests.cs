using FluentAssertions;
using TemplateApi.Domain.Entities;

namespace TemplateApi.UnitTests.Domain;

public class ProductTests
{
    [Fact]
    public void Create_WithValidData_ShouldReturnProduct()
    {
        // Arrange
        var name = "Test Product";
        var description = "Test Description";
        var price = 10.50m;

        // Act
        var product = Product.Create(name, description, price);

        // Assert
        product.Should().NotBeNull();
        product.Name.Should().Be(name);
        product.Description.Should().Be(description);
        product.Price.Should().Be(price);
        product.IsActive.Should().BeTrue();
        product.Id.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WithInvalidName_ShouldThrowArgumentException(string? name)
    {
        // Act
        Action act = () => Product.Create(name!, "Description", 10m);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_WithValidData_ShouldUpdateProduct()
    {
        // Arrange
        var product = Product.Create("Old Name", "Old Description", 5m);
        var newName = "New Name";
        var newDescription = "New Description";
        var newPrice = 20m;

        // Act
        product.Update(newName, newDescription, newPrice);

        // Assert
        product.Name.Should().Be(newName);
        product.Description.Should().Be(newDescription);
        product.Price.Should().Be(newPrice);
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveToFalse()
    {
        // Arrange
        var product = Product.Create("Product", "Description", 10m);

        // Act
        product.Deactivate();

        // Assert
        product.IsActive.Should().BeFalse();
    }
}
