namespace YmmoApi.Services.Interfaces;

public interface IReportService
{
    Task<byte[]> GenerateSalesExcelAsync(int? agencyId, DateOnly? from, DateOnly? to, string? reportType);
    Task<byte[]> GenerateSalesPdfAsync(int? agencyId, DateOnly? from, DateOnly? to, string? reportType);
}
