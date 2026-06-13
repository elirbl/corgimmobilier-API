namespace YmmoApi.Models;

public class Transaction
{
    public int Id { get; set; }
    public int PropertyId { get; set; }
    public Property? Property { get; set; }
    public int ClientId { get; set; }
    public User? Client { get; set; }
    public int? AgentId { get; set; }
    public User? Agent { get; set; }
    public TransactionStage CurrentStage { get; set; } = TransactionStage.Interest;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<TransactionStageHistory> StageHistory { get; set; } = [];
    public ICollection<TransactionDocument> Documents { get; set; } = [];
}
