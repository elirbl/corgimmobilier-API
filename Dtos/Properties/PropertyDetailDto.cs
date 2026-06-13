using YmmoApi.Dtos.Agencies;
using YmmoApi.Models;

namespace YmmoApi.Dtos.Properties;

public class PropertyDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public PropertyType Type { get; set; }
    public PropertyStatus Status { get; set; }
    public string City { get; set; } = string.Empty;
    public int Bedrooms { get; set; }
    public double Area { get; set; }
    public string? ImageUrl { get; set; }
    public PropertyDpe? DpeRating { get; set; }
    public DateOnly ListedDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int AgencyId { get; set; }
    public string AgencyName { get; set; } = string.Empty;
    public AgentSummaryDto? Agent { get; set; }
    public List<PhotoDto> Photos { get; set; } = [];
}
