using YmmoApi.Models;

namespace YmmoApi.Services.Interfaces;

public interface IDashboardRepository
{
    Task<Agency?> GetAgencyAsync(int agencyId);
    Task<int> GetPropertiesCountAsync(int? agencyId);
    Task<int> GetSalesCountAsync(int? agencyId);
    Task<decimal> GetTotalRevenueAsync(int? agencyId);
    Task<int> GetVisitsCountAsync(int? agencyId);
    Task<int> GetAgentsCountAsync(int agencyId);
    Task<List<AgencyMonthlyRevenue>> GetMonthlyRevenueAsync(int? agencyId);
}
