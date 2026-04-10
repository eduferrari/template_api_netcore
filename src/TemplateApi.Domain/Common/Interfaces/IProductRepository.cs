using TemplateApi.Domain.Entities;

namespace TemplateApi.Domain.Common.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Product>> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
    Task AddAsync(Product entity, CancellationToken ct = default);
    Task UpdateAsync(Product entity, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}