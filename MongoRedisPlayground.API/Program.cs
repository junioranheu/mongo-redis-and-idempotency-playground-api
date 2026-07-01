using MongoRedisPlayground.API;
using MongoRedisPlayground.Application;
using MongoRedisPlayground.Domain.Consts;
using MongoRedisPlayground.Infrastructure;

Console.Title = SystemConsts.App.NameApi;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddDependencyInjectionAPI(builder);
    builder.Services.AddDependencyInjectionApplication();
    builder.Services.AddDependencyInjectionInfrastructure(builder);
}

WebApplication app = builder.Build();
{
    await app.UseAppConfiguration(builder);
    app.Run();
}