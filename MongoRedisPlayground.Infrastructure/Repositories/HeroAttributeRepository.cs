using MongoDB.Driver;
using MongoRedisPlayground.Domain.Entities;
using MongoRedisPlayground.Domain.Repositories;
using MongoRedisPlayground.Infrastructure.Persistence;

namespace MongoRedisPlayground.Infrastructure.Repositories;

public sealed class HeroAttributeRepository(MongoDbContext context) : IHeroAttributeRepository
{
    private readonly MongoDbContext _context = context;

    public async Task<List<HeroAttribute>> GetAllAsync()
    {
        return await _context.HeroAttributes.Find(_ => true).ToListAsync();
    }

    public async Task<HeroAttribute?> GetByIdAsync(string id)
    {
        return await _context.HeroAttributes.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<HeroAttribute?> GetByNameAsync(string name)
    {
        return await _context.HeroAttributes.Find(x => x.Name == name).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(HeroAttribute heroAttribute)
    {
        await _context.HeroAttributes.InsertOneAsync(heroAttribute);
    }

    public async Task UpdateAsync(HeroAttribute heroAttribute)
    {
        await _context.HeroAttributes.ReplaceOneAsync(x => x.Id == heroAttribute.Id, heroAttribute);
    }

    public async Task DeleteAsync(string id)
    {
        await _context.HeroAttributes.DeleteOneAsync(x => x.Id == id);
    }
}
