namespace YmmoApi.Models;

public class Property
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public PropertyType Type { get; set; }
    public PropertyStatus Status { get; set; }
    public int AgencyId { get; set; }
    public Agency? Agency { get; set; }
    public string City { get; set; } = string.Empty;
    public int Bedrooms { get; set; }
    public double Area { get; set; }
    public string? ImageUrl { get; set; }
    public DateOnly ListedDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
}
