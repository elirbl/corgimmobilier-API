using Microsoft.EntityFrameworkCore;
using YmmoApi.Data;
using YmmoApi.Models;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Services;

public class TransactionRepository(YmmoDbContext db) : ITransactionRepository
{
    public Task<Property?> GetPropertyAsync(int propertyId) =>
        db.Properties.FirstOrDefaultAsync(p => p.Id == propertyId);

    public Task<Transaction?> GetByIdAsync(int id) =>
        db.Transactions.FirstOrDefaultAsync(t => t.Id == id);

    public Task<Transaction?> GetByIdWithDetailsAsync(int id) =>
        db.Transactions
            .Include(t => t.Property)
            .Include(t => t.Client)
            .Include(t => t.Agent)
            .Include(t => t.StageHistory)
            .Include(t => t.Documents)
            .FirstOrDefaultAsync(t => t.Id == id);

    public Task<List<Transaction>> GetMineAsync(int userId, UserRole role)
    {
        var query = db.Transactions
            .Include(t => t.Property)
                .ThenInclude(p => p!.Photos)
            .Include(t => t.Client)
            .Include(t => t.Agent)
            .AsQueryable();

        query = role switch
        {
            UserRole.Admin => query,
            UserRole.Agent => query.Where(t => t.AgentId == userId),
            _ => query.Where(t => t.ClientId == userId)
        };

        return query.OrderByDescending(t => t.UpdatedAt ?? t.CreatedAt).ToListAsync();
    }

    public async Task AddAsync(Transaction transaction) =>
        await db.Transactions.AddAsync(transaction);

    public async Task AddStageHistoryAsync(TransactionStageHistory history) =>
        await db.TransactionStageHistories.AddAsync(history);

    public async Task AddDocumentAsync(TransactionDocument document) =>
        await db.TransactionDocuments.AddAsync(document);

    public Task SaveChangesAsync() =>
        db.SaveChangesAsync();
}
