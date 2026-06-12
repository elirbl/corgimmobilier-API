using Microsoft.EntityFrameworkCore;
using YmmoApi.Data;
using YmmoApi.Models;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Services;

public class UserRepository(YmmoDbContext db) : IUserRepository
{
    public Task<User?> GetByEmailAsync(string email) =>
        db.Users.FirstOrDefaultAsync(u => u.Email == email);

    public Task<User?> GetByIdAsync(int id) =>
        db.Users.FirstOrDefaultAsync(u => u.Id == id);

    public async Task AddAsync(User user) =>
        await db.Users.AddAsync(user);

    public async Task AddRefreshTokenAsync(RefreshToken refreshToken) =>
        await db.RefreshTokens.AddAsync(refreshToken);

    public Task<RefreshToken?> GetRefreshTokenAsync(string token) =>
        db.RefreshTokens.Include(rt => rt.User).FirstOrDefaultAsync(rt => rt.Token == token);

    public Task SaveChangesAsync() =>
        db.SaveChangesAsync();
}
