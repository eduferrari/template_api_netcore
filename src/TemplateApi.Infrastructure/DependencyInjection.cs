using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TemplateApi.Domain.Common.Interfaces;
using TemplateApi.Infrastructure.Persistence;
using TemplateApi.Infrastructure.Persistence.Repositories;

namespace TemplateApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services.All(d => d.ServiceType != typeof(DbContextOptions<AppDbContext>)))
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));
        }

        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}