namespace YmmoApi.Dtos.Messages;

public class SendMessageDto
{
    public int RecipientId { get; set; }
    public string Content { get; set; } = string.Empty;
}
