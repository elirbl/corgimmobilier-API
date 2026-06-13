using YmmoApi.Models;

namespace YmmoApi.Services.Interfaces;

public interface IVisitRepository
{
    Task<Property?> GetPropertyAsync(int propertyId);
    Task<User?> GetUserAsync(int userId);
    Task<Visit?> GetByIdAsync(int id);
    Task<Visit?> GetByIdWithDetailsAsync(int id);
    Task<List<Visit>> GetByAgentAsync(int agentId, DateOnly? date);
    Task<List<Visit>> GetActiveByAgentAndDateAsync(int agentId, DateOnly date);
    Task AddAsync(Visit visit);
    Task SaveChangesAsync();
}
