using YmmoApi.Common;
using YmmoApi.Dtos.Messages;

namespace YmmoApi.Services.Interfaces;

public interface IMessageService
{
    Task<List<ConversationSummaryDto>> GetConversationsAsync(int userId);
    Task<ServiceResult<PagedResult<MessageDto>>> GetMessagesAsync(int userId, int otherUserId, int page, int pageSize);
    Task<ServiceResult<MessageDto>> SendMessageAsync(SendMessageDto dto, int senderId);
    Task<int> MarkAsReadAsync(int userId, int otherUserId);
}
