using YmmoApi.Models;

namespace YmmoApi.Dtos.Transactions;

public class TransactionListItemDto
{
    public int Id { get; set; }
    public int PropertyId { get; set; }
    public string PropertyTitle { get; set; } = string.Empty;
    public string? PropertyImageUrl { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string? AgentName { get; set; }
    public TransactionStage CurrentStage { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
