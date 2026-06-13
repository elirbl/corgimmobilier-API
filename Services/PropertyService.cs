using YmmoApi.Common;
using YmmoApi.Dtos.Agencies;
using YmmoApi.Dtos.Properties;
using YmmoApi.Models;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Services;

public class PropertyService(IPropertyRepository repository, IPhotoStorageService photoStorage) : IPropertyService
{
    private const int MaxPhotosPerProperty = 10;
    private const long MaxPhotoSizeBytes = 5 * 1024 * 1024;

    public async Task<PagedResult<PropertyListItemDto>> GetPagedAsync(PropertyFilter filter, int page, int pageSize, string? sort, bool isAuthenticated)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 100 ? 20 : pageSize;

        if (!isAuthenticated && filter.Status is null or PropertyStatus.Available)
        {
            var (viewItems, viewTotal) = await repository.GetPagedFromViewAsync(filter, page, pageSize, sort);

            return new PagedResult<PropertyListItemDto>
            {
                Items = viewItems.Select(p => new PropertyListItemDto
                {
                    Id = p.PropertyId,
                    Title = p.Title,
                    Type = p.Type,
                    Status = PropertyStatus.Available,
                    Price = p.Price,
                    City = p.City,
                    Bedrooms = p.Bedrooms,
                    Area = p.Area,
                    DpeRating = p.DpeRating,
                    ListedDate = p.ListedDate,
                    AgencyId = p.AgencyId,
                    AgencyName = p.AgencyName
                }).ToList(),
                Page = page,
                PageSize = pageSize,
                TotalCount = viewTotal
            };
        }

        var (items, totalCount) = await repository.GetPagedAsync(filter, page, pageSize, sort);

        return new PagedResult<PropertyListItemDto>
        {
            Items = items.Select(ToListItemDto).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<PropertyDetailDto?> GetDetailAsync(int id)
    {
        var property = await repository.GetByIdWithDetailsAsync(id);
        return property is null ? null : ToDetailDto(property);
    }

    public async Task<PropertyDetailDto> CreateAsync(PropertyCreateDto dto)
    {
        var property = new Property
        {
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price,
            Type = dto.Type,
            Status = dto.Status,
            AgencyId = dto.AgencyId,
            City = dto.City,
            Bedrooms = dto.Bedrooms,
            Area = dto.Area,
            ImageUrl = dto.ImageUrl,
            DpeRating = dto.DpeRating,
            AgentId = dto.AgentId
        };

        await repository.AddAsync(property);
        await repository.SaveChangesAsync();

        var created = await repository.GetByIdWithDetailsAsync(property.Id);
        return ToDetailDto(created!);
    }

    public async Task<bool> UpdateAsync(int id, PropertyUpdateDto dto)
    {
        var property = await repository.GetByIdAsync(id);
        if (property is null)
            return false;

        property.Title = dto.Title;
        property.Description = dto.Description;
        property.Price = dto.Price;
        property.Type = dto.Type;
        property.Status = dto.Status;
        property.AgencyId = dto.AgencyId;
        property.City = dto.City;
        property.Bedrooms = dto.Bedrooms;
        property.Area = dto.Area;
        property.ImageUrl = dto.ImageUrl;
        property.DpeRating = dto.DpeRating;
        property.AgentId = dto.AgentId;
        property.UpdatedAt = DateTime.UtcNow;

        await repository.SaveChangesAsync();
        return true;
    }

    public async Task<ServiceResult> UpdateStatusAsync(int id, PropertyStatusUpdateDto dto)
    {
        var property = await repository.GetByIdAsync(id);
        if (property is null)
            return ServiceResult.Failure("Bien introuvable.");

        property.Status = dto.Status;
        property.UpdatedAt = DateTime.UtcNow;

        await repository.SaveChangesAsync();
        return ServiceResult.Success();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var property = await repository.GetByIdAsync(id);
        if (property is null)
            return false;

        repository.Remove(property);
        await repository.SaveChangesAsync();
        return true;
    }

    public async Task<ServiceResult<PropertyPhotoUploadResultDto>> AddPhotosAsync(int id, IReadOnlyList<IFormFile> files)
    {
        var property = await repository.GetByIdAsync(id);
        if (property is null)
            return ServiceResult<PropertyPhotoUploadResultDto>.Failure("Bien introuvable.");

        if (files.Count == 0)
            return ServiceResult<PropertyPhotoUploadResultDto>.Failure("Aucun fichier fourni.");

        var existingCount = await repository.GetPhotoCountAsync(id);
        var result = new PropertyPhotoUploadResultDto();

        foreach (var file in files)
        {
            if (existingCount + result.Added.Count >= MaxPhotosPerProperty)
            {
                result.Errors.Add($"{file.FileName} : nombre maximum de {MaxPhotosPerProperty} photos atteint.");
                continue;
            }

            if (file.Length > MaxPhotoSizeBytes)
            {
                result.Errors.Add($"{file.FileName} : la taille dépasse la limite de 5 Mo.");
                continue;
            }

            if (file.Length == 0)
            {
                result.Errors.Add($"{file.FileName} : fichier vide.");
                continue;
            }

            var url = await photoStorage.SaveAsync(file);
            var photo = new Photo
            {
                PropertyId = id,
                Url = url,
                IsMain = existingCount + result.Added.Count == 0
            };

            await repository.AddPhotoAsync(photo);
            result.Added.Add(new PhotoDto { Id = photo.Id, Url = photo.Url, IsMain = photo.IsMain });
        }

        if (result.Added.Count > 0)
            await repository.SaveChangesAsync();

        return ServiceResult<PropertyPhotoUploadResultDto>.Success(result);
    }

    public async Task<ServiceResult> DeletePhotoAsync(int id, int photoId)
    {
        var photo = await repository.GetPhotoAsync(id, photoId);
        if (photo is null)
            return ServiceResult.Failure("Photo introuvable.");

        photoStorage.Delete(photo.Url);
        repository.RemovePhoto(photo);
        await repository.SaveChangesAsync();
        return ServiceResult.Success();
    }

    private static PropertyListItemDto ToListItemDto(Property p) => new()
    {
        Id = p.Id,
        Title = p.Title,
        Type = p.Type,
        Status = p.Status,
        Price = p.Price,
        City = p.City,
        Bedrooms = p.Bedrooms,
        Area = p.Area,
        DpeRating = p.DpeRating,
        ImageUrl = p.ImageUrl,
        ListedDate = p.ListedDate,
        AgencyId = p.AgencyId,
        AgencyName = p.Agency?.Name ?? string.Empty
    };

    private static PropertyDetailDto ToDetailDto(Property p) => new()
    {
        Id = p.Id,
        Title = p.Title,
        Description = p.Description,
        Price = p.Price,
        Type = p.Type,
        Status = p.Status,
        City = p.City,
        Bedrooms = p.Bedrooms,
        Area = p.Area,
        ImageUrl = p.ImageUrl,
        DpeRating = p.DpeRating,
        ListedDate = p.ListedDate,
        CreatedAt = p.CreatedAt,
        UpdatedAt = p.UpdatedAt,
        AgencyId = p.AgencyId,
        AgencyName = p.Agency?.Name ?? string.Empty,
        Agent = p.Agent is null ? null : new AgentSummaryDto
        {
            Id = p.Agent.Id,
            FirstName = p.Agent.FirstName,
            LastName = p.Agent.LastName,
            Email = p.Agent.Email,
            Phone = p.Agent.Phone
        },
        Photos = p.Photos.Select(ph => new PhotoDto { Id = ph.Id, Url = ph.Url, IsMain = ph.IsMain }).ToList()
    };
}
