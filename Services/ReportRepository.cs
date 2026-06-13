using Microsoft.EntityFrameworkCore;
using YmmoApi.Data;
using YmmoApi.Dtos.Reports;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Services;

public class ReportRepository(YmmoDbContext db) : IReportRepository
{
    public async Task<List<SaleReportRowDto>> GetSalesDetailAsync(int? agencyId, DateOnly? from, DateOnly? to)
    {
        return await db.Sales
            .Where(s => agencyId == null || s.Property!.AgencyId == agencyId)
            .Where(s => from == null || s.Date >= from)
            .Where(s => to == null || s.Date <= to)
            .OrderBy(s => s.Date)
            .Select(s => new SaleReportRowDto
            {
                Id = s.Id,
                PropertyTitle = s.Property!.Title,
                AgencyName = s.Property!.Agency!.Name,
                BuyerName = s.Buyer == null ? string.Empty : $"{s.Buyer.FirstName} {s.Buyer.LastName}",
                SellerName = s.Seller == null ? string.Empty : $"{s.Seller.FirstName} {s.Seller.LastName}",
                SalePrice = s.SalePrice,
                Date = s.Date,
                Comment = s.Comment
            })
            .ToListAsync();
    }

    public async Task<List<AgencySalesSummaryDto>> GetSalesSummaryByAgencyAsync(int? agencyId, DateOnly? from, DateOnly? to)
    {
        return await db.Sales
            .Where(s => agencyId == null || s.Property!.AgencyId == agencyId)
            .Where(s => from == null || s.Date >= from)
            .Where(s => to == null || s.Date <= to)
            .GroupBy(s => new { s.Property!.AgencyId, s.Property!.Agency!.Name })
            .Select(g => new AgencySalesSummaryDto
            {
                AgencyId = g.Key.AgencyId,
                AgencyName = g.Key.Name,
                SalesCount = g.Count(),
                TotalRevenue = g.Sum(s => s.SalePrice),
                AverageSalePrice = g.Average(s => s.SalePrice)
            })
            .OrderBy(a => a.AgencyName)
            .ToListAsync();
    }
}
