using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineNet.Application.Common.Abstractions;
using OnlineNet.Domain.Baskets;
using OnlineNet.Domain.Customers;
using OnlineNet.Domain.Orders;
using OnlineNet.Domain.Products;
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
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IBasketRepository, BasketRepository>();

        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
