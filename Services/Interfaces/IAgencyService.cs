using YmmoApi.Common;
using YmmoApi.Dtos.Agencies;

namespace YmmoApi.Services.Interfaces;

public interface IAgencyService
{
    Task<PagedResult<AgencyResponseDto>> GetPagedAsync(string? city, int page, int pageSize);
    Task<AgencyDetailDto?> GetDetailAsync(int id);
    Task<AgencyResponseDto> CreateAsync(AgencyCreateDto dto);
    Task<bool> UpdateAsync(int id, AgencyUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<ServiceResult> AttachAgentAsync(int agencyId, int agentId);
}

public class ServiceResult
{
    public bool Succeeded { get; init; }
    public bool IsForbidden { get; init; }
    public string? Error { get; init; }

    public static ServiceResult Success() => new() { Succeeded = true };
    public static ServiceResult Failure(string error) => new() { Succeeded = false, Error = error };
    public static ServiceResult Forbidden(string error) => new() { Succeeded = false, IsForbidden = true, Error = error };
}
