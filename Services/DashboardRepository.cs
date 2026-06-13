using Microsoft.EntityFrameworkCore;
using YmmoApi.Data;
using YmmoApi.Models;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Services;

public class DashboardRepository(YmmoDbContext db) : IDashboardRepository
{
    public Task<Agency?> GetAgencyAsync(int agencyId) =>
        db.Agencies.FirstOrDefaultAsync(a => a.Id == agencyId);

    public Task<int> GetPropertiesCountAsync(int? agencyId) =>
        db.Properties.CountAsync(p => agencyId == null || p.AgencyId == agencyId);

    public Task<int> GetSalesCountAsync(int? agencyId) =>
        db.Sales.CountAsync(s => agencyId == null || s.Property!.AgencyId == agencyId);

    public Task<decimal> GetTotalRevenueAsync(int? agencyId) =>
        db.Sales
            .Where(s => agencyId == null || s.Property!.AgencyId == agencyId)
            .SumAsync(s => s.SalePrice);

    public Task<int> GetVisitsCountAsync(int? agencyId) =>
        db.Visits.CountAsync(v => agencyId == null || v.Property!.AgencyId == agencyId);

    public Task<int> GetAgentsCountAsync(int agencyId) =>
        db.Users.CountAsync(u => u.Role == UserRole.Agent && u.AgencyId == agencyId);

    public async Task<List<AgencyMonthlyRevenue>> GetMonthlyRevenueAsync(int? agencyId)
    {
        return await db.AgencyMonthlyRevenues.FromSqlInterpolated($@"
            WITH ventes_agence AS (
                SELECT
                    p.""AgencyId"",
                    a.""Name"" AS ""AgencyName"",
                    DATE_TRUNC('month', s.""Date"")::date AS ""Month"",
                    SUM(s.""SalePrice"") AS ""Revenue""
                FROM ""Sales"" s
                JOIN ""Properties"" p ON p.""Id"" = s.""PropertyId""
                JOIN ""Agencies"" a ON a.""Id"" = p.""AgencyId""
                WHERE s.""Date"" >= (CURRENT_DATE - INTERVAL '12 months')
                  AND ({agencyId}::int IS NULL OR p.""AgencyId"" = {agencyId}::int)
                GROUP BY p.""AgencyId"", a.""Name"", DATE_TRUNC('month', s.""Date"")
            )
            SELECT
                ""AgencyId"",
                ""AgencyName"",
                ""Month"",
                ""Revenue"",
                RANK() OVER (PARTITION BY ""AgencyId"" ORDER BY ""Revenue"" DESC) AS ""RankInAgency""
            FROM ventes_agence
            ORDER BY ""AgencyId"", ""Month""")
            .ToListAsync();
    }
}
