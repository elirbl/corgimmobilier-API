namespace YmmoApi.Dtos.Dashboard;

public class AgencyMonthlyRevenueDto
{
    public int AgencyId { get; set; }
    public string AgencyName { get; set; } = string.Empty;
    public DateOnly Month { get; set; }
    public decimal Revenue { get; set; }
    public long RankInAgency { get; set; }
}
