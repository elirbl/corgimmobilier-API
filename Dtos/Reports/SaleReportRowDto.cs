namespace YmmoApi.Dtos.Reports;

public class SaleReportRowDto
{
    public int Id { get; set; }
    public string PropertyTitle { get; set; } = string.Empty;
    public string AgencyName { get; set; } = string.Empty;
    public string BuyerName { get; set; } = string.Empty;
    public string SellerName { get; set; } = string.Empty;
    public decimal SalePrice { get; set; }
    public DateOnly Date { get; set; }
    public string Comment { get; set; } = string.Empty;
}
