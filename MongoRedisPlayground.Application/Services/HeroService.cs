using Microsoft.AspNetCore.Http;
using MongoRedisPlayground.Domain.Consts;
using MongoRedisPlayground.Domain.Entities;
using MongoRedisPlayground.Domain.Repositories;
using MongoRedisPlayground.Domain.Services;
using System.Text.Json;

namespace MongoRedisPlayground.Application.Services;

public sealed class HeroService(IHeroRepository repository, ICacheService cacheService, IIdempotencyRepository idempotencyRepository)
{
    private readonly IHeroRepository _repository = repository;
    private readonly ICacheService _cacheService = cacheService;
    private readonly IIdempotencyRepository _idempotencyRepository = idempotencyRepository;

    public async Task<List<Hero>> GetAllAsync()
    {
        List<Hero>? heroes = await _cacheService.GetAsync<List<Hero>>(CacheKeys.Heroes.All);

        if (heroes is not null)
        {
            return heroes;
        }

        heroes = await _repository.GetAllAsync();

        await _cacheService.SetAsync(key: CacheKeys.Heroes.All, value: heroes, expiration: TimeSpan.FromMinutes(5));

        return heroes;
    }

    public async Task<Hero?> GetByIdAsync(string id)
    {
        Hero? hero = await _cacheService.GetAsync<Hero>(key: CacheKeys.Heroes.ById(id));

        if (hero is not null)
        {
            return hero;
        }

        hero = await _repository.GetByIdAsync(id);

        if (hero is not null)
        {
            await _cacheService.SetAsync(key: CacheKeys.Heroes.ById(id), value: hero, expiration: TimeSpan.FromMinutes(5));
        }

        return hero;
    }

    public async Task<Hero> CreateAsync(Hero hero, string idempotencyKey)
    {
        IdempotencyRequest? existingRequest = await _idempotencyRepository.GetByKeyAsync(idempotencyKey);

        if (existingRequest is not null)
        {
            Hero? cachedHero = JsonSerializer.Deserialize<Hero>(existingRequest.ResponseBody);

            return cachedHero ?? throw new InvalidOperationException("Invalid idempotency response.");
        }

        Hero? existingHero = await _repository.GetByNameAsync(hero.Name);

        if (existingHero is not null)
        {
            throw new InvalidOperationException("Hero already exists.");
        }

        await _repository.CreateAsync(hero);

        await _idempotencyRepository.CreateAsync(new IdempotencyRequest
        {
            Key = idempotencyKey,
            Endpoint = $"{nameof(HeroService)}.{nameof(CreateAsync)}",
            StatusCode = StatusCodes.Status201Created,
            ResponseBody = JsonSerializer.Serialize(hero),
            CreatedAt = DateTime.UtcNow
        });

        await _cacheService.RemoveAsync(key: CacheKeys.Heroes.All);

        return hero;
    }

    public async Task UpdateAsync(Hero hero)
    {
        await _repository.UpdateAsync(hero);

        await _cacheService.RemoveAsync(key: CacheKeys.Heroes.All);

        await _cacheService.RemoveAsync(key: CacheKeys.Heroes.ById(hero.Id));
    }

    public async Task DeleteAsync(string id)
    {
        await _repository.DeleteAsync(id);

        await _cacheService.RemoveAsync(key: CacheKeys.Heroes.All);

        await _cacheService.RemoveAsync(key: CacheKeys.Heroes.ById(id));
    }
}