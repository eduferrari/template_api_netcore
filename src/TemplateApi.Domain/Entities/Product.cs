using TemplateApi.Domain.Common;

namespace TemplateApi.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public bool IsActive { get; private set; } = true;

    protected Product() { }

    public static Product Create(string name, string description, decimal price)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return new Product
        {
            Name = name,
            Description = description,
            Price = price
        };
    }

    public void Update(string name, string description, decimal price)
    {
        Name = name;
        Description = description;
        Price = price;
        SetUpdatedAt();
    }

    public void Deactivate() => IsActive = false;
}