using Microsoft.EntityFrameworkCore;
using YmmoApi.Data;
using YmmoApi.Models;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Services;

public class VisitRepository(YmmoDbContext db) : IVisitRepository
{
    public Task<Property?> GetPropertyAsync(int propertyId) =>
        db.Properties.FirstOrDefaultAsync(p => p.Id == propertyId);

    public Task<User?> GetUserAsync(int userId) =>
        db.Users.FirstOrDefaultAsync(u => u.Id == userId);

    public Task<Visit?> GetByIdAsync(int id) =>
        db.Visits.FirstOrDefaultAsync(v => v.Id == id);

    public Task<Visit?> GetByIdWithDetailsAsync(int id) =>
        db.Visits
            .Include(v => v.Property)
            .Include(v => v.Client)
            .Include(v => v.Agent)
            .FirstOrDefaultAsync(v => v.Id == id);

    public Task<List<Visit>> GetByAgentAsync(int agentId, DateOnly? date)
    {
        var query = db.Visits
            .Include(v => v.Property)
            .Include(v => v.Client)
            .Include(v => v.Agent)
            .Where(v => v.AgentId == agentId);

        if (date.HasValue)
        {
            var start = date.Value.ToDateTime(TimeOnly.MinValue);
            var end = start.AddDays(1);
            query = query.Where(v => v.ScheduledAt >= start && v.ScheduledAt < end);
        }

        return query.OrderBy(v => v.ScheduledAt).ToListAsync();
    }

    public Task<List<Visit>> GetActiveByAgentAndDateAsync(int agentId, DateOnly date)
    {
        var start = date.ToDateTime(TimeOnly.MinValue);
        var end = start.AddDays(1);

        return db.Visits
            .Where(v => v.AgentId == agentId
                && v.ScheduledAt >= start && v.ScheduledAt < end
                && v.Status != VisitStatus.Cancelled)
            .ToListAsync();
    }

    public async Task AddAsync(Visit visit) =>
        await db.Visits.AddAsync(visit);

    public Task SaveChangesAsync() =>
        db.SaveChangesAsync();
}
