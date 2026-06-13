using YmmoApi.Models;

namespace YmmoApi.Services.Interfaces;

public interface ITransactionRepository
{
    Task<Property?> GetPropertyAsync(int propertyId);
    Task<Transaction?> GetByIdAsync(int id);
    Task<Transaction?> GetByIdWithDetailsAsync(int id);
    Task AddAsync(Transaction transaction);
    Task AddStageHistoryAsync(TransactionStageHistory history);
    Task AddDocumentAsync(TransactionDocument document);
    Task SaveChangesAsync();
}
