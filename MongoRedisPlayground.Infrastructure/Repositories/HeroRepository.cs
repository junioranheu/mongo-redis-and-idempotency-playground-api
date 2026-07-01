using MongoDB.Driver;
using MongoRedisPlayground.Domain.Entities;
using MongoRedisPlayground.Domain.Repositories;
using MongoRedisPlayground.Infrastructure.Persistence;

namespace MongoRedisPlayground.Infrastructure.Repositories;

public sealed class HeroRepository(MongoDbContext context) : IHeroRepository
{
    private readonly MongoDbContext _context = context;

    public async Task<List<Hero>> GetAllAsync()
    {
        return await _context.Heroes.Find(_ => true).ToListAsync();
    }

    public async Task<Hero?> GetByIdAsync(string id)
    {
        return await _context.Heroes.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Hero?> GetByNameAsync(string name)
    {
        return await _context.Heroes.Find(x => x.Name == name).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Hero hero)
    {
        await _context.Heroes.InsertOneAsync(hero);
    }

    public async Task UpdateAsync(Hero hero)
    {
        await _context.Heroes.ReplaceOneAsync(x => x.Id == hero.Id, hero);
    }

    public async Task DeleteAsync(string id)
    {
        await _context.Heroes.DeleteOneAsync(x => x.Id == id);
    }
}