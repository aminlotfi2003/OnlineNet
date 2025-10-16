using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineNet.Application.Common.Abstractions;
using OnlineNet.Domain.Catalog.Products;
using OnlineNet.Infrastructure.Persistence;
using OnlineNet.Infrastructure.Persistence.Repositories;

namespace OnlineNet.Infrastructure.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default")));

        // Register Repositories
        services.AddScoped<IProductRepository, ProductRepository>();

        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
