namespace YmmoApi.Services.Interfaces;

public interface IDocumentStorageService
{
    Task<string> SaveAsync(IFormFile file);
}
