using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoRedisPlayground.Domain.Repositories;
using MongoRedisPlayground.Domain.Services;
using MongoRedisPlayground.Infrastructure.Persistence;
using MongoRedisPlayground.Infrastructure.Repositories;
using MongoRedisPlayground.Infrastructure.Services;
using StackExchange.Redis;

namespace MongoRedisPlayground.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencyInjectionInfrastructure(this IServiceCollection services, WebApplicationBuilder builder)
    {
        // AddServices(services, builder);
        // AddAuth(services, builder);
        // AddFactory(services, builder);
        // AddRabbitMQ(services);
        // AddContext(services, builder);
        // AddJobs(services);
        // AddNugetPackages();
        AddMongoDb(services, builder);
        AddRedis(services, builder);
        AddServices(services);
        AddRepositories(services);

        return services;
    }

    /// <summary>
    /// Configures MongoDB dependencies used by the application.
    ///
    /// The MongoDB cluster is hosted on MongoDB Atlas and can be managed through:
    /// https://cloud.mongodb.com/v2/6a31f47cd2ad55932b17556e#/overview?automateSecurity=true
    ///
    /// Registers:
    /// - MongoDbSettings
    /// - IMongoClient
    /// - MongoDbContext
    ///
    /// Collections are created automatically by MongoDB when the first document
    /// is inserted.
    /// </summary>
    private static void AddMongoDb(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDb"));

        services.AddSingleton<IMongoClient>(x =>
        {
            string connectionString = builder.Configuration["MongoDb:ConnectionString"] ?? string.Empty;

            return new MongoClient(connectionString);
        });

        services.AddSingleton<MongoDbContext>();
    }

    /// <summary>
    /// Configures Redis dependencies used by the application.
    ///
    /// The Redis instance is hosted on Upstash and can be managed through:
    /// https://console.upstash.com/redis/04d360cf-1f4b-405f-8f96-8cf68cad5288/details?teamid=0
    ///
    /// Registers:
    /// - IConnectionMultiplexer
    /// - IDistributedCache
    /// - Cache services
    ///
    /// Redis is used as a distributed cache to reduce the number of requests
    /// sent to MongoDB and improve application performance.
    /// </summary>
    private static void AddRedis(IServiceCollection services, WebApplicationBuilder builder)
    {
        string connectionString = builder.Configuration["Redis:ConnectionString"] ?? string.Empty;

        ConfigurationOptions options = ConfigurationOptions.Parse(connectionString);

        options.AbortOnConnectFail = false;
        options.ConnectRetry = 5;
        options.ConnectTimeout = 10000;
        options.SyncTimeout = 10000;

        services.AddStackExchangeRedisCache(x =>
        {
            x.ConfigurationOptions = options;
        });
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddSingleton<ICacheService, RedisCacheService>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IHeroRepository, HeroRepository>();
        services.AddScoped<IHeroAttributeRepository, HeroAttributeRepository>();
        services.AddScoped<IIdempotencyRepository, IdempotencyRepository>();
    }
}