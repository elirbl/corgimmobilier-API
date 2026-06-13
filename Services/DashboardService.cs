using YmmoApi.Dtos.Dashboard;
using YmmoApi.Models;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Services;

public class DashboardService(IDashboardRepository repository) : IDashboardService
{
    public async Task<GlobalDashboardDto> GetGlobalAsync()
    {
        var totalRevenue = await repository.GetTotalRevenueAsync(null);
        var salesCount = await repository.GetSalesCountAsync(null);
        var propertiesCount = await repository.GetPropertiesCountAsync(null);
        var visitsCount = await repository.GetVisitsCountAsync(null);
        var revenueByAgency = await repository.GetMonthlyRevenueAsync(null);

        return new GlobalDashboardDto
        {
            TotalRevenue = totalRevenue,
            SalesCount = salesCount,
            PropertiesCount = propertiesCount,
            ConversionRate = ComputeConversionRate(salesCount, visitsCount),
            RevenueByAgency = revenueByAgency.Select(ToAgencyMonthlyRevenueDto).ToList()
        };
    }

    public async Task<ServiceResult<AgencyDashboardDto>> GetAgencyAsync(int agencyId)
    {
        var agency = await repository.GetAgencyAsync(agencyId);
        if (agency is null)
            return ServiceResult<AgencyDashboardDto>.Failure("Agence introuvable.");

        var totalRevenue = await repository.GetTotalRevenueAsync(agencyId);
        var salesCount = await repository.GetSalesCountAsync(agencyId);
        var propertiesCount = await repository.GetPropertiesCountAsync(agencyId);
        var visitsCount = await repository.GetVisitsCountAsync(agencyId);
        var agentsCount = await repository.GetAgentsCountAsync(agencyId);
        var monthlyRevenue = await repository.GetMonthlyRevenueAsync(agencyId);

        return ServiceResult<AgencyDashboardDto>.Success(new AgencyDashboardDto
        {
            AgencyId = agency.Id,
            AgencyName = agency.Name,
            TotalRevenue = totalRevenue,
            SalesCount = salesCount,
            PropertiesCount = propertiesCount,
            AgentsCount = agentsCount,
            ConversionRate = ComputeConversionRate(salesCount, visitsCount),
            MonthlyRevenue = monthlyRevenue.Select(ToMonthlyRevenueDto).ToList()
        });
    }

    private static double ComputeConversionRate(int salesCount, int visitsCount) =>
        visitsCount == 0 ? 0 : Math.Round((double)salesCount / visitsCount * 100, 2);

    private static AgencyMonthlyRevenueDto ToAgencyMonthlyRevenueDto(AgencyMonthlyRevenue r) => new()
    {
        AgencyId = r.AgencyId,
        AgencyName = r.AgencyName,
        Month = r.Month,
        Revenue = r.Revenue,
        RankInAgency = r.RankInAgency
    };

    private static MonthlyRevenueDto ToMonthlyRevenueDto(AgencyMonthlyRevenue r) => new()
    {
        Month = r.Month,
        Revenue = r.Revenue,
        RankInAgency = r.RankInAgency
    };
}
