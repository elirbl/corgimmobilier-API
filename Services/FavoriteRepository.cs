using Microsoft.EntityFrameworkCore;
using YmmoApi.Data;
using YmmoApi.Models;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Services;

public class FavoriteRepository(YmmoDbContext db) : IFavoriteRepository
{
    public Task<List<Favorite>> GetByUserAsync(int userId) =>
        db.Favorites
            .Include(f => f.Property)
                .ThenInclude(p => p!.Agency)
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();

    public Task<Favorite?> GetAsync(int userId, int propertyId) =>
        db.Favorites.FirstOrDefaultAsync(f => f.UserId == userId && f.PropertyId == propertyId);

    public Task<Property?> GetPropertyAsync(int propertyId) =>
        db.Properties.FirstOrDefaultAsync(p => p.Id == propertyId);

    public async Task AddAsync(Favorite favorite) =>
        await db.Favorites.AddAsync(favorite);

    public void Remove(Favorite favorite) =>
        db.Favorites.Remove(favorite);

    public Task SaveChangesAsync() =>
        db.SaveChangesAsync();
}
