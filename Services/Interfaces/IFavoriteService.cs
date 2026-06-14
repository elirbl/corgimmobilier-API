using YmmoApi.Dtos.Favorites;

namespace YmmoApi.Services.Interfaces;

public interface IFavoriteService
{
    Task<List<FavoriteDto>> GetByUserAsync(int userId);
    Task<ServiceResult<FavoriteDto>> AddAsync(int userId, int propertyId);
    Task<ServiceResult> RemoveAsync(int userId, int propertyId);
}
