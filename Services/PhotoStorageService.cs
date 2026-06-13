using YmmoApi.Services.Interfaces;

namespace YmmoApi.Services;

public class PhotoStorageService(IWebHostEnvironment env) : IPhotoStorageService
{
    private const string UploadsFolder = "uploads";

    public async Task<string> SaveAsync(IFormFile file)
    {
        var webRoot = string.IsNullOrEmpty(env.WebRootPath)
            ? Path.Combine(env.ContentRootPath, "wwwroot")
            : env.WebRootPath;

        var uploadsPath = Path.Combine(webRoot, UploadsFolder);
        Directory.CreateDirectory(uploadsPath);

        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsPath, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/{UploadsFolder}/{fileName}";
    }

    public void Delete(string url)
    {
        var webRoot = string.IsNullOrEmpty(env.WebRootPath)
            ? Path.Combine(env.ContentRootPath, "wwwroot")
            : env.WebRootPath;

        var fileName = Path.GetFileName(url);
        var filePath = Path.Combine(webRoot, UploadsFolder, fileName);

        if (File.Exists(filePath))
            File.Delete(filePath);
    }
}
