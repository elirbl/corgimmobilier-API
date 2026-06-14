using YmmoApi.Models;

namespace YmmoApi.Services.Interfaces;

public record PropertyFilter
{
    public PropertyType? Type { get; init; }
    public PropertyStatus? Status { get; init; }
    public decimal? MinPrice { get; init; }
    public decimal? MaxPrice { get; init; }
    public double? MinArea { get; init; }
    public string? City { get; init; }
    public PropertyDpe? DpeRating { get; init; }
    public int? AgentId { get; init; }
}

public interface IPropertyRepository
{
    Task<(List<Property> Items, int TotalCount)> GetPagedAsync(PropertyFilter filter, int page, int pageSize, string? sort);
    Task<(List<AvailablePropertyListing> Items, int TotalCount)> GetPagedFromViewAsync(PropertyFilter filter, int page, int pageSize, string? sort);
    Task<Property?> GetByIdAsync(int id);
    Task<Property?> GetByIdWithDetailsAsync(int id);
    Task AddAsync(Property property);
    void Remove(Property property);
    Task<int> GetPhotoCountAsync(int propertyId);
    Task<Photo?> GetPhotoAsync(int propertyId, int photoId);
    Task AddPhotoAsync(Photo photo);
    void RemovePhoto(Photo photo);
    Task SaveChangesAsync();
}
