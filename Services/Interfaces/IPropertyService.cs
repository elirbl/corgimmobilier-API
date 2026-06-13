using YmmoApi.Common;
using YmmoApi.Dtos.Properties;

namespace YmmoApi.Services.Interfaces;

public interface IPropertyService
{
    Task<PagedResult<PropertyListItemDto>> GetPagedAsync(PropertyFilter filter, int page, int pageSize, string? sort, bool isAuthenticated);
    Task<PropertyDetailDto?> GetDetailAsync(int id);
    Task<PropertyDetailDto> CreateAsync(PropertyCreateDto dto);
    Task<bool> UpdateAsync(int id, PropertyUpdateDto dto);
    Task<ServiceResult> UpdateStatusAsync(int id, PropertyStatusUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<ServiceResult<PropertyPhotoUploadResultDto>> AddPhotosAsync(int id, IReadOnlyList<IFormFile> files);
    Task<ServiceResult> DeletePhotoAsync(int id, int photoId);
}
