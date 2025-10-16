using OnlineNet.Application.Extensions.DependencyInjection;
using OnlineNet.Infrastructure.Extensions.DependencyInjection;

namespace OnlineNet.WebApp.Extensions.DependencyInjection;

public static class SeviceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register Dependencies Layers
        services
            .AddApplication()
            .AddPersistence(configuration);

        // Register Services
        services.AddAuthentication();
        services.AddHttpContextAccessor();

        return services;
    }
}
