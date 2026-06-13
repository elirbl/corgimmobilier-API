using Microsoft.EntityFrameworkCore;
using YmmoApi.Data;
using YmmoApi.Models;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Services;

public class AgencyRepository(YmmoDbContext db) : IAgencyRepository
{
    public async Task<(List<Agency> Items, int TotalCount)> GetPagedAsync(string? city, int page, int pageSize)
    {
        var query = db.Agencies.AsQueryable();

        if (!string.IsNullOrWhiteSpace(city))
            query = query.Where(a => a.City.ToLower().Contains(city.ToLower()));

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(a => a.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public Task<Agency?> GetByIdAsync(int id) =>
        db.Agencies.FirstOrDefaultAsync(a => a.Id == id);

    public Task<List<User>> GetAgentsAsync(int agencyId) =>
        db.Users.Where(u => u.AgencyId == agencyId && u.Role == UserRole.Agent).ToListAsync();

    public Task<User?> GetUserByIdAsync(int userId) =>
        db.Users.FirstOrDefaultAsync(u => u.Id == userId);

    public Task<int> GetPropertiesCountAsync(int agencyId) =>
        db.Properties.CountAsync(p => p.AgencyId == agencyId);

    public async Task AddAsync(Agency agency) =>
        await db.Agencies.AddAsync(agency);

    public void Remove(Agency agency) =>
        db.Agencies.Remove(agency);

    public Task SaveChangesAsync() =>
        db.SaveChangesAsync();
}
