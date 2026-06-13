using YmmoApi.Models;

namespace YmmoApi.Dtos.Properties;

public class PropertyListItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public PropertyType Type { get; set; }
    public PropertyStatus? Status { get; set; }
    public decimal Price { get; set; }
    public string City { get; set; } = string.Empty;
    public int Bedrooms { get; set; }
    public double Area { get; set; }
    public PropertyDpe? DpeRating { get; set; }
    public string? ImageUrl { get; set; }
    public DateOnly ListedDate { get; set; }
    public int AgencyId { get; set; }
    public string AgencyName { get; set; } = string.Empty;
}
