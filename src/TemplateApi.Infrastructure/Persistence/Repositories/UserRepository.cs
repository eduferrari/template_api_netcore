using Microsoft.EntityFrameworkCore;
using TemplateApi.Domain.Common.Interfaces;
using TemplateApi.Domain.Entities;

namespace TemplateApi.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
        => _context = context;

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Set<User>().FindAsync(new object[] { id }, ct);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        => await _context.Set<User>().FirstOrDefaultAsync(x => x.Email == email, ct);

    public async Task AddAsync(User entity, CancellationToken ct = default)
    {
        await _context.Set<User>().AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(User entity, CancellationToken ct = default)
    {
        _context.Set<User>().Update(entity);
        await _context.SaveChangesAsync(ct);
    }
}
