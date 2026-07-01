using MongoRedisPlayground.Domain.Consts;
using MongoRedisPlayground.Domain.Entities;
using MongoRedisPlayground.Domain.Repositories;
using MongoRedisPlayground.Domain.Services;

namespace MongoRedisPlayground.Application.Services;

public sealed class HeroAttributeService(IHeroAttributeRepository repository, ICacheService cacheService)
{
    private readonly IHeroAttributeRepository _repository = repository;
    private readonly ICacheService _cacheService = cacheService;

    public async Task<List<HeroAttribute>> GetAllAsync()
    {
        List<HeroAttribute>? heroAttributes = await _cacheService.GetAsync<List<HeroAttribute>>(CacheKeys.HeroAttributes.All);

        if (heroAttributes is not null)
        {
            return heroAttributes;
        }

        heroAttributes = await _repository.GetAllAsync();

        await _cacheService.SetAsync(key: CacheKeys.HeroAttributes.All, value: heroAttributes, expiration: TimeSpan.FromMinutes(5));

        return heroAttributes;
    }

    public async Task<HeroAttribute?> GetByIdAsync(string id)
    {
        HeroAttribute? heroAttribute = await _cacheService.GetAsync<HeroAttribute>(key: CacheKeys.HeroAttributes.ById(id));

        if (heroAttribute is not null)
        {
            return heroAttribute;
        }

        heroAttribute = await _repository.GetByIdAsync(id);

        if (heroAttribute is not null)
        {
            await _cacheService.SetAsync(key: CacheKeys.HeroAttributes.ById(id), value: heroAttribute, expiration: TimeSpan.FromMinutes(5));
        }

        return heroAttribute;
    }

    public async Task CreateAsync(HeroAttribute heroAttribute)
    {
        HeroAttribute? existing = await _repository.GetByNameAsync(heroAttribute.Name);

        if (existing is not null)
        {
            throw new InvalidOperationException("Hero attribute already exists.");
        }

        await _repository.CreateAsync(heroAttribute);

        await _cacheService.RemoveAsync(key: CacheKeys.HeroAttributes.All);
    }

    public async Task UpdateAsync(HeroAttribute heroAttribute)
    {
        await _repository.UpdateAsync(heroAttribute);

        await _cacheService.RemoveAsync(key: CacheKeys.HeroAttributes.All);

        await _cacheService.RemoveAsync(key: CacheKeys.HeroAttributes.ById(heroAttribute.Id));
    }

    public async Task DeleteAsync(string id)
    {
        await _repository.DeleteAsync(id);

        await _cacheService.RemoveAsync(key: CacheKeys.HeroAttributes.All);

        await _cacheService.RemoveAsync(key: CacheKeys.HeroAttributes.ById(id));
    }
}