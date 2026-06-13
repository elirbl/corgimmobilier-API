using YmmoApi.Dtos.Agencies;
using YmmoApi.Dtos.Transactions;
using YmmoApi.Models;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Services;

public class TransactionService(
    ITransactionRepository repository,
    IUserRepository userRepository,
    IDocumentStorageService documentStorage,
    IEmailService emailService) : ITransactionService
{
    private static readonly string[] AllowedDocumentExtensions = [".pdf"];
    private const long MaxDocumentSizeBytes = 10 * 1024 * 1024;

    public async Task<ServiceResult<TransactionDetailDto>> CreateAsync(TransactionCreateDto dto, int clientId)
    {
        var property = await repository.GetPropertyAsync(dto.PropertyId);
        if (property is null)
            return ServiceResult<TransactionDetailDto>.Failure("Bien introuvable.");

        var transaction = new Transaction
        {
            PropertyId = dto.PropertyId,
            ClientId = clientId,
            CurrentStage = TransactionStage.Interest
        };

        await repository.AddAsync(transaction);
        await repository.SaveChangesAsync();

        await repository.AddStageHistoryAsync(new TransactionStageHistory
        {
            TransactionId = transaction.Id,
            Stage = TransactionStage.Interest,
            Notes = dto.Notes
        });
        await repository.SaveChangesAsync();

        var client = await userRepository.GetByIdAsync(clientId);
        if (client is not null)
        {
            await emailService.SendAsync(
                client.Email,
                "Confirmation de votre demande",
                $"Bonjour {client.FirstName},\n\nVotre demande pour le bien \"{property.Title}\" a bien été enregistrée. " +
                "Un agent prendra contact avec vous prochainement.\n\nL'équipe Ymmo.");
        }

        var created = await repository.GetByIdWithDetailsAsync(transaction.Id);
        return ServiceResult<TransactionDetailDto>.Success(ToDetailDto(created!));
    }

    public async Task<ServiceResult<TransactionDetailDto>> GetDetailAsync(int id, int currentUserId, UserRole currentRole)
    {
        var transaction = await repository.GetByIdWithDetailsAsync(id);
        if (transaction is null)
            return ServiceResult<TransactionDetailDto>.Failure("Transaction introuvable.");

        if (!CanAccess(transaction, currentUserId, currentRole))
            return ServiceResult<TransactionDetailDto>.Forbidden("Accès refusé à cette transaction.");

        return ServiceResult<TransactionDetailDto>.Success(ToDetailDto(transaction));
    }

    public async Task<ServiceResult<TransactionDetailDto>> AdvanceStageAsync(int id, TransactionStageUpdateDto dto, int agentId)
    {
        var transaction = await repository.GetByIdAsync(id);
        if (transaction is null)
            return ServiceResult<TransactionDetailDto>.Failure("Transaction introuvable.");

        if (transaction.CurrentStage == TransactionStage.Deed)
            return ServiceResult<TransactionDetailDto>.Failure("La transaction a déjà atteint l'étape finale (Acte).");

        transaction.CurrentStage = (TransactionStage)((int)transaction.CurrentStage + 1);
        transaction.UpdatedAt = DateTime.UtcNow;
        transaction.AgentId ??= agentId;

        await repository.AddStageHistoryAsync(new TransactionStageHistory
        {
            TransactionId = transaction.Id,
            Stage = transaction.CurrentStage,
            Notes = dto.Notes
        });
        await repository.SaveChangesAsync();

        var updated = await repository.GetByIdWithDetailsAsync(id);
        return ServiceResult<TransactionDetailDto>.Success(ToDetailDto(updated!));
    }

    public async Task<ServiceResult<TransactionDocumentDto>> AddDocumentAsync(int id, IFormFile file, int currentUserId, UserRole currentRole)
    {
        var transaction = await repository.GetByIdAsync(id);
        if (transaction is null)
            return ServiceResult<TransactionDocumentDto>.Failure("Transaction introuvable.");

        if (!CanAccess(transaction, currentUserId, currentRole))
            return ServiceResult<TransactionDocumentDto>.Forbidden("Accès refusé à cette transaction.");

        if (file.Length == 0)
            return ServiceResult<TransactionDocumentDto>.Failure("Fichier vide.");

        if (file.Length > MaxDocumentSizeBytes)
            return ServiceResult<TransactionDocumentDto>.Failure("La taille du fichier dépasse la limite de 10 Mo.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedDocumentExtensions.Contains(extension))
            return ServiceResult<TransactionDocumentDto>.Failure("Seuls les fichiers PDF sont acceptés.");

        var url = await documentStorage.SaveAsync(file);
        var document = new TransactionDocument
        {
            TransactionId = id,
            FileName = file.FileName,
            Url = url
        };

        await repository.AddDocumentAsync(document);
        await repository.SaveChangesAsync();

        return ServiceResult<TransactionDocumentDto>.Success(new TransactionDocumentDto
        {
            Id = document.Id,
            FileName = document.FileName,
            Url = document.Url,
            UploadedAt = document.UploadedAt
        });
    }

    private static bool CanAccess(Transaction transaction, int currentUserId, UserRole currentRole) =>
        currentRole == UserRole.Admin
        || transaction.ClientId == currentUserId
        || transaction.AgentId == currentUserId;

    private static TransactionDetailDto ToDetailDto(Transaction t) => new()
    {
        Id = t.Id,
        PropertyId = t.PropertyId,
        PropertyTitle = t.Property?.Title ?? string.Empty,
        Client = new AgentSummaryDto
        {
            Id = t.Client!.Id,
            FirstName = t.Client.FirstName,
            LastName = t.Client.LastName,
            Email = t.Client.Email,
            Phone = t.Client.Phone
        },
        Agent = t.Agent is null ? null : new AgentSummaryDto
        {
            Id = t.Agent.Id,
            FirstName = t.Agent.FirstName,
            LastName = t.Agent.LastName,
            Email = t.Agent.Email,
            Phone = t.Agent.Phone
        },
        CurrentStage = t.CurrentStage,
        CreatedAt = t.CreatedAt,
        UpdatedAt = t.UpdatedAt,
        StageHistory = t.StageHistory
            .OrderBy(h => h.ChangedAt)
            .Select(h => new TransactionStageHistoryDto { Stage = h.Stage, ChangedAt = h.ChangedAt, Notes = h.Notes })
            .ToList(),
        Documents = t.Documents
            .OrderBy(d => d.UploadedAt)
            .Select(d => new TransactionDocumentDto { Id = d.Id, FileName = d.FileName, Url = d.Url, UploadedAt = d.UploadedAt })
            .ToList()
    };
}
