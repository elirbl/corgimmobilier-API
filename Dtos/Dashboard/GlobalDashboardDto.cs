namespace YmmoApi.Dtos.Dashboard;

public class GlobalDashboardDto
{
    public decimal TotalRevenue { get; set; }
    public int SalesCount { get; set; }
    public int PropertiesCount { get; set; }
    public double ConversionRate { get; set; }
    public List<AgencyMonthlyRevenueDto> RevenueByAgency { get; set; } = [];
}
