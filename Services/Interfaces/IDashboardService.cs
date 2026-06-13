using YmmoApi.Dtos.Dashboard;

namespace YmmoApi.Services.Interfaces;

public interface IDashboardService
{
    Task<GlobalDashboardDto> GetGlobalAsync();
    Task<ServiceResult<AgencyDashboardDto>> GetAgencyAsync(int agencyId);
}
