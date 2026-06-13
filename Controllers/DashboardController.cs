using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YmmoApi.Common;
using YmmoApi.Dtos.Dashboard;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize(Roles = "Admin")]
public class DashboardController(IDashboardService dashboardService) : ControllerBase
{
    [HttpGet("global")]
    public async Task<ActionResult<ApiResponse<GlobalDashboardDto>>> GetGlobal()
    {
        var data = await dashboardService.GetGlobalAsync();
        return Ok(ApiResponse<GlobalDashboardDto>.Ok(data));
    }

    [HttpGet("agence/{id:int}")]
    public async Task<ActionResult<ApiResponse<AgencyDashboardDto>>> GetAgency(int id)
    {
        var result = await dashboardService.GetAgencyAsync(id);
        if (!result.Succeeded)
            return NotFound(ApiResponse<AgencyDashboardDto>.Fail(result.Error!));

        return Ok(ApiResponse<AgencyDashboardDto>.Ok(result.Data!));
    }
}
