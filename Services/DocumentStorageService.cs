using YmmoApi.Services.Interfaces;

namespace YmmoApi.Services;

public class DocumentStorageService(IWebHostEnvironment env) : IDocumentStorageService
{
    private const string UploadsFolder = "uploads/documents";

    public async Task<string> SaveAsync(IFormFile file)
    {
        var webRoot = string.IsNullOrEmpty(env.WebRootPath)
            ? Path.Combine(env.ContentRootPath, "wwwroot")
            : env.WebRootPath;

        var uploadsPath = Path.Combine(webRoot, "uploads", "documents");
        Directory.CreateDirectory(uploadsPath);

        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsPath, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/{UploadsFolder}/{fileName}";
    }
}
