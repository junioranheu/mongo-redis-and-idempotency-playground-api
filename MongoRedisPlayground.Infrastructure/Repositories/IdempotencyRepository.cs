using MongoDB.Driver;
using MongoRedisPlayground.Domain.Entities;
using MongoRedisPlayground.Domain.Repositories;
using MongoRedisPlayground.Infrastructure.Persistence;

namespace MongoRedisPlayground.Infrastructure.Repositories;

public sealed class IdempotencyRepository(MongoDbContext context) : IIdempotencyRepository
{
    public async Task<IdempotencyRequest?> GetByKeyAsync(string key)
    {
        return await context.IdempotencyRequests.Find(x => x.Key == key).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(IdempotencyRequest request)
    {
        await context.IdempotencyRequests.InsertOneAsync(request);
    }
}