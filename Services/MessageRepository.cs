using Microsoft.EntityFrameworkCore;
using YmmoApi.Data;
using YmmoApi.Models;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Services;

public class MessageRepository(YmmoDbContext db) : IMessageRepository
{
    public Task<User?> GetUserAsync(int userId) =>
        db.Users.FirstOrDefaultAsync(u => u.Id == userId);

    public Task<List<Message>> GetAllForUserAsync(int userId) =>
        db.Messages
            .Include(m => m.Sender)
            .Include(m => m.Recipient)
            .Where(m => m.SenderId == userId || m.RecipientId == userId)
            .OrderByDescending(m => m.SentAt)
            .ToListAsync();

    public async Task<(List<Message> Items, int TotalCount)> GetConversationMessagesAsync(int userId, int otherUserId, int page, int pageSize)
    {
        var query = db.Messages
            .Include(m => m.Sender)
            .Include(m => m.Recipient)
            .Where(m =>
                (m.SenderId == userId && m.RecipientId == otherUserId) ||
                (m.SenderId == otherUserId && m.RecipientId == userId));

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(m => m.SentAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task AddAsync(Message message) =>
        await db.Messages.AddAsync(message);

    public async Task<int> MarkAsReadAsync(int userId, int otherUserId)
    {
        var unread = await db.Messages
            .Where(m => m.RecipientId == userId && m.SenderId == otherUserId && !m.IsRead)
            .ToListAsync();

        foreach (var message in unread)
            message.IsRead = true;

        return unread.Count;
    }

    public Task SaveChangesAsync() =>
        db.SaveChangesAsync();
}
