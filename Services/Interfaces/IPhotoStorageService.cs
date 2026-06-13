namespace YmmoApi.Services.Interfaces;

public interface IPhotoStorageService
{
    Task<string> SaveAsync(IFormFile file);
    void Delete(string url);
}
