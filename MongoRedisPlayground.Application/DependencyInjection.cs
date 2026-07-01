using Microsoft.Extensions.DependencyInjection;
using MongoRedisPlayground.Application.Services;

namespace MongoRedisPlayground.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencyInjectionApplication(this IServiceCollection services)
    {
        // AddLogger(builder);
        // AddUseCases(services);
        AddServices(services);
        // AddHandlers(services);

        return services;
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<HeroService>();
        services.AddScoped<HeroAttributeService>();
    }
}