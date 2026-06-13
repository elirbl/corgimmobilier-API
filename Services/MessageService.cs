using YmmoApi.Common;
using YmmoApi.Dtos.Messages;
using YmmoApi.Models;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Services;

public class MessageService(IMessageRepository repository) : IMessageService
{
    public async Task<List<ConversationSummaryDto>> GetConversationsAsync(int userId)
    {
        var messages = await repository.GetAllForUserAsync(userId);

        return messages
            .GroupBy(m => m.SenderId == userId ? m.RecipientId : m.SenderId)
            .Select(g =>
            {
                var last = g.First();
                var other = last.SenderId == userId ? last.Recipient : last.Sender;

                return new ConversationSummaryDto
                {
                    UserId = g.Key,
                    UserName = other is null ? string.Empty : $"{other.FirstName} {other.LastName}",
                    LastMessage = last.Content,
                    LastMessageAt = last.SentAt,
                    UnreadCount = g.Count(m => m.RecipientId == userId && !m.IsRead)
                };
            })
            .OrderByDescending(c => c.LastMessageAt)
            .ToList();
    }

    public async Task<ServiceResult<PagedResult<MessageDto>>> GetMessagesAsync(int userId, int otherUserId, int page, int pageSize)
    {
        var otherUser = await repository.GetUserAsync(otherUserId);
        if (otherUser is null)
            return ServiceResult<PagedResult<MessageDto>>.Failure("Utilisateur introuvable.");

        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 100 ? 20 : pageSize;

        var (items, totalCount) = await repository.GetConversationMessagesAsync(userId, otherUserId, page, pageSize);

        return ServiceResult<PagedResult<MessageDto>>.Success(new PagedResult<MessageDto>
        {
            Items = items.Select(ToDto).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        });
    }

    public async Task<ServiceResult<MessageDto>> SendMessageAsync(SendMessageDto dto, int senderId)
    {
        if (dto.RecipientId == senderId)
            return ServiceResult<MessageDto>.Failure("Impossible de s'envoyer un message à soi-même.");

        var recipient = await repository.GetUserAsync(dto.RecipientId);
        if (recipient is null)
            return ServiceResult<MessageDto>.Failure("Destinataire introuvable.");

        var sender = await repository.GetUserAsync(senderId);

        var message = new Message
        {
            SenderId = senderId,
            RecipientId = dto.RecipientId,
            Content = dto.Content
        };

        await repository.AddAsync(message);
        await repository.SaveChangesAsync();

        message.Sender = sender;
        message.Recipient = recipient;

        return ServiceResult<MessageDto>.Success(ToDto(message));
    }

    public async Task<int> MarkAsReadAsync(int userId, int otherUserId)
    {
        var count = await repository.MarkAsReadAsync(userId, otherUserId);
        if (count > 0)
            await repository.SaveChangesAsync();

        return count;
    }

    private static MessageDto ToDto(Message m) => new()
    {
        Id = m.Id,
        SenderId = m.SenderId,
        SenderName = m.Sender is null ? string.Empty : $"{m.Sender.FirstName} {m.Sender.LastName}",
        RecipientId = m.RecipientId,
        RecipientName = m.Recipient is null ? string.Empty : $"{m.Recipient.FirstName} {m.Recipient.LastName}",
        Content = m.Content,
        SentAt = m.SentAt,
        IsRead = m.IsRead
    };
}
