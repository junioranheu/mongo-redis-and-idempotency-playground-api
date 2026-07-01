using MongoRedisPlayground.Domain.Entities;

namespace MongoRedisPlayground.Domain.Repositories;

public interface IHeroRepository
{
    Task<List<Hero>> GetAllAsync();

    Task<Hero?> GetByIdAsync(string id);

    Task<Hero?> GetByNameAsync(string name);

    Task CreateAsync(Hero hero);

    Task UpdateAsync(Hero hero);

    Task DeleteAsync(string id);
}