namespace YmmoApi.Dtos.Reports;

public class AgencySalesSummaryDto
{
    public int AgencyId { get; set; }
    public string AgencyName { get; set; } = string.Empty;
    public int SalesCount { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal AverageSalePrice { get; set; }
}
