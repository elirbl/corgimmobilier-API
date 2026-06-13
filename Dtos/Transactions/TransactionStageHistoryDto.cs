using YmmoApi.Models;

namespace YmmoApi.Dtos.Transactions;

public class TransactionStageHistoryDto
{
    public TransactionStage Stage { get; set; }
    public DateTime ChangedAt { get; set; }
    public string? Notes { get; set; }
}
