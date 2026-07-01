using MongoRedisPlayground.Domain.Entities;

namespace MongoRedisPlayground.Domain.Repositories;

public interface IIdempotencyRepository
{
    Task<IdempotencyRequest?> GetByKeyAsync(string key);
    Task CreateAsync(IdempotencyRequest request);
}