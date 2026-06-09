using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YmmoApi.Data;
using YmmoApi.Models;

namespace YmmoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AgenciesController(YmmoDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Agency>>> GetAll() =>
        Ok(await db.Agencies.ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Agency>> GetById(int id)
    {
        var agency = await db.Agencies.FindAsync(id);
        return agency is null ? NotFound() : Ok(agency);
    }

    [HttpPost]
    public async Task<ActionResult<Agency>> Create(Agency agency)
    {
        db.Agencies.Add(agency);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = agency.Id }, agency);
    }
}
