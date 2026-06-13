using YmmoApi.Models;

namespace YmmoApi.Services.Interfaces;

public interface IAgencyRepository
{
    Task<(List<Agency> Items, int TotalCount)> GetPagedAsync(string? city, int page, int pageSize);
    Task<Agency?> GetByIdAsync(int id);
    Task<List<User>> GetAgentsAsync(int agencyId);
    Task<User?> GetUserByIdAsync(int userId);
    Task<int> GetPropertiesCountAsync(int agencyId);
    Task AddAsync(Agency agency);
    void Remove(Agency agency);
    Task SaveChangesAsync();
}
