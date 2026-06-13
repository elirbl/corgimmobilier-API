using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using YmmoApi.Dtos.Messages;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Hubs;

[Authorize]
public class ChatHub(IMessageService messageService) : Hub
{
    public async Task JoinConversation(int otherUserId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, GetConversationGroup(GetUserId(), otherUserId));
    }

    public async Task SendMessage(int recipientId, string content)
    {
        var senderId = GetUserId();

        var result = await messageService.SendMessageAsync(new SendMessageDto { RecipientId = recipientId, Content = content }, senderId);
        if (!result.Succeeded)
        {
            await Clients.Caller.SendAsync("ErrorMessage", result.Error);
            return;
        }

        var message = result.Data!;
        var groupName = GetConversationGroup(senderId, recipientId);

        await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        await Clients.User(recipientId.ToString()).SendAsync("ReceiveMessage", message);
        await Clients.User(recipientId.ToString()).SendAsync("UnreadMessage", message);
    }

    public async Task MarkAsRead(int otherUserId)
    {
        var userId = GetUserId();
        var count = await messageService.MarkAsReadAsync(userId, otherUserId);

        if (count > 0)
        {
            var groupName = GetConversationGroup(userId, otherUserId);
            await Clients.Group(groupName).SendAsync("MessagesRead", new { ReaderId = userId, OtherUserId = otherUserId });
        }
    }

    private int GetUserId() => int.Parse(Context.UserIdentifier!);

    private static string GetConversationGroup(int userA, int userB)
    {
        var (a, b) = userA < userB ? (userA, userB) : (userB, userA);
        return $"conversation-{a}-{b}";
    }
}
