using MongoRedisPlayground.Domain.Entities;

namespace MongoRedisPlayground.Domain.Repositories;

public interface IHeroAttributeRepository
{
    Task<List<HeroAttribute>> GetAllAsync();

    Task<HeroAttribute?> GetByIdAsync(string id);

    Task<HeroAttribute?> GetByNameAsync(string name);

    Task CreateAsync(HeroAttribute heroAttribute);

    Task UpdateAsync(HeroAttribute heroAttribute);

    Task DeleteAsync(string id);
}