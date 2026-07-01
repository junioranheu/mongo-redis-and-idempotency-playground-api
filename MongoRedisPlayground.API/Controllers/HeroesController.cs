using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoRedisPlayground.Application.Services;
using MongoRedisPlayground.Domain.Entities;

namespace MongoRedisPlayground.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class HeroesController(HeroService heroService) : ControllerBase
{
    private readonly HeroService _heroService = heroService;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<Hero>>> GetAll()
    {
        List<Hero> heroes = await _heroService.GetAllAsync();

        return Ok(heroes);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<Hero>> GetById(string id)
    {
        Hero? hero = await _heroService.GetByIdAsync(id);

        if (hero is null)
        {
            return NotFound();
        }

        return Ok(hero);
    }

    /// <summary>
    /// Creates a new hero using an Idempotency-Key sent in the request header.
    /// The front-end should generate this key, usually with crypto.randomUUID(),
    /// and send it in the request headers to avoid duplicated operations in retries,
    /// timeouts, or accidental double submissions.
    /// </summary>
    /// <remarks>
    /// Example:
    ///
    /// const idempotencyKey = crypto.randomUUID();
    ///
    /// await fetch("https://localhost:7092/api/heroes", {
    ///     method: "POST",
    ///     headers: {
    ///         "Content-Type": "application/json",
    ///         "Idempotency-Key": idempotencyKey
    ///     },
    ///     body: JSON.stringify({
    ///         name: "Batman",
    ///         attributeId: "68644e9d5d8d2f9b3c8c9b11"
    ///     })
    /// });
    /// </remarks>
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<Hero>> Create(Hero hero)
    {
        string? idempotencyKey = Request.Headers["Idempotency-Key"];

        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            return BadRequest("Header Idempotency-Key is required.");
        }

        Hero result = await _heroService.CreateAsync(hero, idempotencyKey);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut]
    [AllowAnonymous]
    public async Task<IActionResult> Update(Hero hero)
    {
        await _heroService.UpdateAsync(hero);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Delete(string id)
    {
        await _heroService.DeleteAsync(id);

        return NoContent();
    }
}