using YmmoApi.Models;

namespace YmmoApi.Services.Interfaces;

public interface IMessageRepository
{
    Task<User?> GetUserAsync(int userId);
    Task<List<Message>> GetAllForUserAsync(int userId);
    Task<(List<Message> Items, int TotalCount)> GetConversationMessagesAsync(int userId, int otherUserId, int page, int pageSize);
    Task AddAsync(Message message);
    Task<int> MarkAsReadAsync(int userId, int otherUserId);
    Task SaveChangesAsync();
}
