using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoRedisPlayground.Application.Services;
using MongoRedisPlayground.Domain.Entities;

namespace MongoRedisPlayground.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class HeroAttributesController(HeroAttributeService heroAttributeService) : ControllerBase
{
    private readonly HeroAttributeService _heroAttributeService = heroAttributeService;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<HeroAttribute>>> GetAll()
    {
        List<HeroAttribute> heroAttributes = await _heroAttributeService.GetAllAsync();

        return Ok(heroAttributes);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<HeroAttribute>> GetById(string id)
    {
        HeroAttribute? heroAttribute = await _heroAttributeService.GetByIdAsync(id);

        if (heroAttribute is null)
        {
            return NotFound();
        }

        return Ok(heroAttribute);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create(HeroAttribute heroAttribute)
    {
        await _heroAttributeService.CreateAsync(heroAttribute);

        return CreatedAtAction(nameof(GetById), new { id = heroAttribute.Id }, heroAttribute);
    }

    [HttpPut]
    [AllowAnonymous]
    public async Task<IActionResult> Update(HeroAttribute heroAttribute)
    {
        await _heroAttributeService.UpdateAsync(heroAttribute);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Delete(string id)
    {
        await _heroAttributeService.DeleteAsync(id);

        return NoContent();
    }
}
