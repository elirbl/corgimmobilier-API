using YmmoApi.Dtos.Favorites;
using YmmoApi.Dtos.Properties;
using YmmoApi.Models;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Services;

public class FavoriteService(IFavoriteRepository repository) : IFavoriteService
{
    public async Task<List<FavoriteDto>> GetByUserAsync(int userId)
    {
        var favorites = await repository.GetByUserAsync(userId);
        return favorites.Select(ToDto).ToList();
    }

    public async Task<ServiceResult<FavoriteDto>> AddAsync(int userId, int propertyId)
    {
        var property = await repository.GetPropertyAsync(propertyId);
        if (property is null)
            return ServiceResult<FavoriteDto>.Failure("Bien introuvable.");

        var existing = await repository.GetAsync(userId, propertyId);
        if (existing is not null)
            return ServiceResult<FavoriteDto>.Success(ToDto(existing, property));

        var favorite = new Favorite { UserId = userId, PropertyId = propertyId };
        await repository.AddAsync(favorite);
        await repository.SaveChangesAsync();

        return ServiceResult<FavoriteDto>.Success(ToDto(favorite, property));
    }

    public async Task<ServiceResult> RemoveAsync(int userId, int propertyId)
    {
        var favorite = await repository.GetAsync(userId, propertyId);
        if (favorite is null)
            return ServiceResult.Failure("Favori introuvable.");

        repository.Remove(favorite);
        await repository.SaveChangesAsync();
        return ServiceResult.Success();
    }

    private static FavoriteDto ToDto(Favorite favorite) => ToDto(favorite, favorite.Property!);

    private static FavoriteDto ToDto(Favorite favorite, Property property) => new()
    {
        Id = favorite.Id,
        CreatedAt = favorite.CreatedAt,
        Property = new PropertyListItemDto
        {
            Id = property.Id,
            Title = property.Title,
            Type = property.Type,
            Status = property.Status,
            Price = property.Price,
            City = property.City,
            Bedrooms = property.Bedrooms,
            Area = property.Area,
            DpeRating = property.DpeRating,
            ImageUrl = property.ImageUrl,
            ListedDate = property.ListedDate,
            AgencyId = property.AgencyId,
            AgencyName = property.Agency?.Name ?? string.Empty,
            AgentId = property.AgentId
        }
    };
}
