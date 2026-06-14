using YmmoApi.Models;

namespace YmmoApi.Services.Interfaces;

public interface IFavoriteRepository
{
    Task<List<Favorite>> GetByUserAsync(int userId);
    Task<Favorite?> GetAsync(int userId, int propertyId);
    Task<Property?> GetPropertyAsync(int propertyId);
    Task AddAsync(Favorite favorite);
    void Remove(Favorite favorite);
    Task SaveChangesAsync();
}
