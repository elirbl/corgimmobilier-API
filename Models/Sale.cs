namespace YmmoApi.Models;

public class Sale
{
    public int Id { get; set; }
    public int PropertyId { get; set; }
    public Property? Property { get; set; }
    public int BuyerId { get; set; }
    public Client? Buyer { get; set; }
    public int SellerId { get; set; }
    public Client? Seller { get; set; }
    public decimal SalePrice { get; set; }
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public string Comment { get; set; } = string.Empty;
}
