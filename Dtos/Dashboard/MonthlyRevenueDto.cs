namespace YmmoApi.Dtos.Dashboard;

public class MonthlyRevenueDto
{
    public DateOnly Month { get; set; }
    public decimal Revenue { get; set; }
    public long RankInAgency { get; set; }
}
