using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoRedisPlayground.Domain.Entities;

namespace MongoRedisPlayground.Infrastructure.Persistence;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        MongoClient client = new(settings.Value.ConnectionString);

        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<Hero> Heroes => _database.GetCollection<Hero>(name: "heroes");

    public IMongoCollection<HeroAttribute> HeroAttributes => _database.GetCollection<HeroAttribute>(name: "hero-attributes");

    public IMongoCollection<IdempotencyRequest> IdempotencyRequests => _database.GetCollection<IdempotencyRequest>(name: "idempotency_requests");

    /// <summary>
    /// Ensures that the MongoDB indexes required by the application exist.
    /// In this case, it creates a unique index for the idempotency key,
    /// preventing the same Idempotency-Key from being stored more than once.
    /// </summary>
    /// <remarks>
    /// This method should be executed when the application starts.
    /// It prepares the database structure and helps guarantee idempotency
    /// even when duplicated requests arrive at the same time.
    /// </remarks>
    public async Task EnsureIndexesAsync()
    {
        CreateIndexModel<IdempotencyRequest> idempotencyKeyIndex = new(
            Builders<IdempotencyRequest>.IndexKeys.Ascending(x => x.Key),
            new CreateIndexOptions { Unique = true }
        );

        await IdempotencyRequests.Indexes.CreateOneAsync(idempotencyKeyIndex);
    }
}