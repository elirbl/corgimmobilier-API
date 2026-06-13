namespace YmmoApi.Models;

public class TransactionStageHistory
{
    public int Id { get; set; }
    public int TransactionId { get; set; }
    public Transaction? Transaction { get; set; }
    public TransactionStage Stage { get; set; }
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }
}
