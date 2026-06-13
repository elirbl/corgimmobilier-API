using YmmoApi.Dtos.Reports;

namespace YmmoApi.Services.Interfaces;

public interface IReportRepository
{
    Task<List<SaleReportRowDto>> GetSalesDetailAsync(int? agencyId, DateOnly? from, DateOnly? to);
    Task<List<AgencySalesSummaryDto>> GetSalesSummaryByAgencyAsync(int? agencyId, DateOnly? from, DateOnly? to);
}
