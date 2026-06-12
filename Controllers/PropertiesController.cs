using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YmmoApi.Data;
using YmmoApi.Models;

namespace YmmoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController(YmmoDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Property>>> GetAll(
        [FromQuery] string? city,
        [FromQuery] PropertyType? type,
        [FromQuery] PropertyStatus? status,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] int? minBedrooms,
        [FromQuery] double? minArea)
    {
        var query = db.Properties.Include(p => p.Agency).AsQueryable();

        if (!string.IsNullOrWhiteSpace(city))
            query = query.Where(p => p.City.ToLower().Contains(city.ToLower()));
        if (type.HasValue)
            query = query.Where(p => p.Type == type.Value);
        if (status.HasValue)
            query = query.Where(p => p.Status == status.Value);
        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice.Value);
        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice.Value);
        if (minBedrooms.HasValue)
            query = query.Where(p => p.Bedrooms >= minBedrooms.Value);
        if (minArea.HasValue)
            query = query.Where(p => p.Area >= minArea.Value);

        return Ok(await query.OrderByDescending(p => p.ListedDate).ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Property>> GetById(int id)
    {
        var property = await db.Properties.Include(p => p.Agency).FirstOrDefaultAsync(p => p.Id == id);
        return property is null ? NotFound() : Ok(property);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Agent")]
    public async Task<ActionResult<Property>> Create(Property property)
    {
        property.ListedDate = DateOnly.FromDateTime(DateTime.UtcNow);
        property.Status = PropertyStatus.Available;
        db.Properties.Add(property);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = property.Id }, property);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,Agent")]
    public async Task<IActionResult> Update(int id, Property property)
    {
        var existing = await db.Properties.FindAsync(id);
        if (existing is null) return NotFound();

        existing.Title = property.Title;
        existing.Description = property.Description;
        existing.Price = property.Price;
        existing.Type = property.Type;
        existing.Status = property.Status;
        existing.AgencyId = property.AgencyId;
        existing.City = property.City;
        existing.Bedrooms = property.Bedrooms;
        existing.Area = property.Area;
        existing.ImageUrl = property.ImageUrl;

        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("cities")]
    public async Task<ActionResult<IEnumerable<string>>> GetCities() =>
        Ok(await db.Properties.Select(p => p.City).Distinct().OrderBy(c => c).ToListAsync());
}
