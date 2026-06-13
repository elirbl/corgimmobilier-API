using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using YmmoApi.Common;
using YmmoApi.Dtos.Messages;
using YmmoApi.Hubs;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Controllers;

[ApiController]
[Route("api/messages")]
[Authorize]
public class MessagesController(
    IMessageService messageService,
    ICurrentUserService currentUser,
    IHubContext<ChatHub> hubContext,
    IValidator<SendMessageDto> sendValidator) : ControllerBase
{
    [HttpGet("conversations")]
    public async Task<ActionResult<ApiResponse<List<ConversationSummaryDto>>>> GetConversations()
    {
        var conversations = await messageService.GetConversationsAsync(currentUser.UserId!.Value);
        return Ok(ApiResponse<List<ConversationSummaryDto>>.Ok(conversations));
    }

    [HttpGet("{conversationId:int}")]
    public async Task<ActionResult<ApiResponse<PagedResult<MessageDto>>>> GetConversation(
        int conversationId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var result = await messageService.GetMessagesAsync(currentUser.UserId!.Value, conversationId, page, pageSize);
        if (!result.Succeeded)
            return NotFound(ApiResponse<PagedResult<MessageDto>>.Fail(result.Error!));

        return Ok(ApiResponse<PagedResult<MessageDto>>.Ok(result.Data!));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<MessageDto>>> SendMessage(SendMessageDto dto)
    {
        var validation = await sendValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return ValidationProblem(BuildModelState(validation));

        var senderId = currentUser.UserId!.Value;
        var result = await messageService.SendMessageAsync(dto, senderId);
        if (!result.Succeeded)
            return BadRequest(ApiResponse<MessageDto>.Fail(result.Error!));

        var message = result.Data!;
        var groupName = GetConversationGroup(senderId, dto.RecipientId);

        await hubContext.Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        await hubContext.Clients.User(dto.RecipientId.ToString()).SendAsync("ReceiveMessage", message);
        await hubContext.Clients.User(dto.RecipientId.ToString()).SendAsync("UnreadMessage", message);

        return Ok(ApiResponse<MessageDto>.Ok(message));
    }

    private static string GetConversationGroup(int userA, int userB)
    {
        var (a, b) = userA < userB ? (userA, userB) : (userB, userA);
        return $"conversation-{a}-{b}";
    }

    private static ModelStateDictionary BuildModelState(FluentValidation.Results.ValidationResult validation)
    {
        var modelState = new ModelStateDictionary();
        foreach (var error in validation.Errors)
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);

        return modelState;
    }
}
