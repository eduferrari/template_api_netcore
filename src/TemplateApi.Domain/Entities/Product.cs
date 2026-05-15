using TemplateApi.Domain.Common;

namespace TemplateApi.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public bool IsActive { get; private set; } = true;

    protected Product() { }

    public static Product Create(string name, string description, decimal price, bool isActive = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return new Product
        {
            Name = name,
            Description = description,
            Price = price,
            IsActive = isActive
        };
    }

    public void Update(string name, string description, decimal price, bool isActive)
    {
        Name = name;
        Description = description;
        Price = price;
        IsActive = isActive;
        SetUpdatedAt();
    }

    public void Deactivate() => IsActive = false;
}