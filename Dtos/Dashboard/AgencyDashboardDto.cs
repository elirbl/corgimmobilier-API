namespace YmmoApi.Dtos.Dashboard;

public class AgencyDashboardDto
{
    public int AgencyId { get; set; }
    public string AgencyName { get; set; } = string.Empty;
    public decimal TotalRevenue { get; set; }
    public int SalesCount { get; set; }
    public int PropertiesCount { get; set; }
    public int AgentsCount { get; set; }
    public double ConversionRate { get; set; }
    public List<MonthlyRevenueDto> MonthlyRevenue { get; set; } = [];
}
