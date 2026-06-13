using Microsoft.EntityFrameworkCore;
using YmmoApi.Data;
using YmmoApi.Models;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Services;

public class PropertyRepository(YmmoDbContext db) : IPropertyRepository
{
    public async Task<(List<Property> Items, int TotalCount)> GetPagedAsync(PropertyFilter filter, int page, int pageSize, string? sort)
    {
        var query = db.Properties.AsQueryable();
        query = ApplyFilter(query, filter);

        var totalCount = await query.CountAsync();

        query = sort switch
        {
            "price" => query.OrderBy(p => p.Price),
            "-price" => query.OrderByDescending(p => p.Price),
            "area" => query.OrderBy(p => p.Area),
            "-area" => query.OrderByDescending(p => p.Area),
            "title" => query.OrderBy(p => p.Title),
            "-title" => query.OrderByDescending(p => p.Title),
            "listedDate" => query.OrderBy(p => p.ListedDate),
            _ => query.OrderByDescending(p => p.ListedDate)
        };

        var items = await query
            .Include(p => p.Agency)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(List<AvailablePropertyListing> Items, int TotalCount)> GetPagedFromViewAsync(PropertyFilter filter, int page, int pageSize, string? sort)
    {
        var query = db.AvailablePropertyListings.AsQueryable();

        if (filter.Type.HasValue)
            query = query.Where(p => p.Type == filter.Type);

        if (filter.MinPrice.HasValue)
            query = query.Where(p => p.Price >= filter.MinPrice);

        if (filter.MaxPrice.HasValue)
            query = query.Where(p => p.Price <= filter.MaxPrice);

        if (filter.MinArea.HasValue)
            query = query.Where(p => p.Area >= filter.MinArea);

        if (!string.IsNullOrWhiteSpace(filter.City))
            query = query.Where(p => p.City.ToLower().Contains(filter.City.ToLower()));

        if (filter.DpeRating.HasValue)
            query = query.Where(p => p.DpeRating == filter.DpeRating);

        var totalCount = await query.CountAsync();

        query = sort switch
        {
            "price" => query.OrderBy(p => p.Price),
            "-price" => query.OrderByDescending(p => p.Price),
            "area" => query.OrderBy(p => p.Area),
            "-area" => query.OrderByDescending(p => p.Area),
            "title" => query.OrderBy(p => p.Title),
            "-title" => query.OrderByDescending(p => p.Title),
            "listedDate" => query.OrderBy(p => p.ListedDate),
            _ => query.OrderByDescending(p => p.ListedDate)
        };

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    private static IQueryable<Property> ApplyFilter(IQueryable<Property> query, PropertyFilter filter)
    {
        if (filter.Type.HasValue)
            query = query.Where(p => p.Type == filter.Type);

        if (filter.Status.HasValue)
            query = query.Where(p => p.Status == filter.Status);

        if (filter.MinPrice.HasValue)
            query = query.Where(p => p.Price >= filter.MinPrice);

        if (filter.MaxPrice.HasValue)
            query = query.Where(p => p.Price <= filter.MaxPrice);

        if (filter.MinArea.HasValue)
            query = query.Where(p => p.Area >= filter.MinArea);

        if (!string.IsNullOrWhiteSpace(filter.City))
            query = query.Where(p => p.City.ToLower().Contains(filter.City.ToLower()));

        if (filter.DpeRating.HasValue)
            query = query.Where(p => p.DpeRating == filter.DpeRating);

        return query;
    }

    public Task<Property?> GetByIdAsync(int id) =>
        db.Properties.FirstOrDefaultAsync(p => p.Id == id);

    public Task<Property?> GetByIdWithDetailsAsync(int id) =>
        db.Properties
            .Include(p => p.Agency)
            .Include(p => p.Agent)
            .Include(p => p.Photos)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task AddAsync(Property property) =>
        await db.Properties.AddAsync(property);

    public void Remove(Property property) =>
        db.Properties.Remove(property);

    public Task<int> GetPhotoCountAsync(int propertyId) =>
        db.Photos.CountAsync(ph => ph.PropertyId == propertyId);

    public Task<Photo?> GetPhotoAsync(int propertyId, int photoId) =>
        db.Photos.FirstOrDefaultAsync(ph => ph.Id == photoId && ph.PropertyId == propertyId);

    public async Task AddPhotoAsync(Photo photo) =>
        await db.Photos.AddAsync(photo);

    public void RemovePhoto(Photo photo) =>
        db.Photos.Remove(photo);

    public Task SaveChangesAsync() =>
        db.SaveChangesAsync();
}
