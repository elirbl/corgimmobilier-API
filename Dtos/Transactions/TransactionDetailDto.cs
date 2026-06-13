using YmmoApi.Dtos.Agencies;
using YmmoApi.Models;

namespace YmmoApi.Dtos.Transactions;

public class TransactionDetailDto
{
    public int Id { get; set; }
    public int PropertyId { get; set; }
    public string PropertyTitle { get; set; } = string.Empty;
    public AgentSummaryDto Client { get; set; } = new();
    public AgentSummaryDto? Agent { get; set; }
    public TransactionStage CurrentStage { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<TransactionStageHistoryDto> StageHistory { get; set; } = [];
    public List<TransactionDocumentDto> Documents { get; set; } = [];
}
