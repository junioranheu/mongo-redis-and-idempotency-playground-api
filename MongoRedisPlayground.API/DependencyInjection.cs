using Microsoft.AspNetCore.ResponseCompression;
using MongoRedisPlayground.Domain.Consts;
using MongoRedisPlayground.Infrastructure.Serialization;
using System.IO.Compression;
using System.Text.Json.Serialization;

namespace MongoRedisPlayground.API;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencyInjectionAPI(this IServiceCollection services, WebApplicationBuilder builder)
    {
        IWebHostEnvironment env = builder.Environment;

        AddSwagger(services);
        AddCors(services, builder);
        AddCompression(services);
        AddControllers(services, env);
        // AddObservability(services);
        AddCaching(services);
        AddHttpContextAccessor(services);
        // AddRateLimiting(services);
        // AddHealthCheck(services, builder);
        AddLogger(builder);

        return services;
    }

    private static void AddSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new()
            {
                Title = SystemConsts.App.NameApi,
                Version = "v1"
            });
        });
    }

    private static void AddCors(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddCors(x =>
            x.AddPolicy(name: builder.Configuration["CORSSettings:Cors"] ?? string.Empty, builder =>
            {
                builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            })
        );
    }

    private static void AddCompression(IServiceCollection services)
    {
        services.AddResponseCompression(x =>
        {
            x.EnableForHttps = true;
            x.Providers.Add<BrotliCompressionProvider>();
            x.Providers.Add<GzipCompressionProvider>();
        });

        services.Configure<BrotliCompressionProviderOptions>(x =>
        {
            x.Level = CompressionLevel.Optimal;
        });

        services.Configure<GzipCompressionProviderOptions>(x =>
        {
            x.Level = CompressionLevel.Optimal;
        });
    }

    private static void AddControllers(IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddControllers().
           AddJsonOptions(x =>
           {
               x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
               x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // Exibir, a descrição do Enum em vez do ID;
               x.JsonSerializerOptions.WriteIndented = env.IsDevelopment();
               x.JsonSerializerOptions.Converters.Add(new BrasiliaDateTimeConverter());
           });
    }

    private static void AddCaching(IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddResponseCaching();
    }

    private static void AddHttpContextAccessor(IServiceCollection services)
    {
        services.AddHttpContextAccessor(); // Serviço necessário para habilitar o IHttpContextAccessor em Infrastructure/Context;
    }

    private static void AddLogger(WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
    }
}