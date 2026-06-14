using YmmoApi.Dtos.Transactions;
using YmmoApi.Models;

namespace YmmoApi.Services.Interfaces;

public interface ITransactionService
{
    Task<ServiceResult<TransactionDetailDto>> CreateAsync(TransactionCreateDto dto, int clientId);
    Task<ServiceResult<TransactionDetailDto>> GetDetailAsync(int id, int currentUserId, UserRole currentRole);
    Task<List<TransactionListItemDto>> GetMineAsync(int currentUserId, UserRole currentRole);
    Task<ServiceResult<TransactionDetailDto>> AdvanceStageAsync(int id, TransactionStageUpdateDto dto, int agentId);
    Task<ServiceResult<TransactionDocumentDto>> AddDocumentAsync(int id, IFormFile file, int currentUserId, UserRole currentRole);
}
