using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Controllers;

[ApiController]
[Route("api/reports/ventes")]
[Authorize(Roles = "Admin")]
public class ReportController(IReportService reportService) : ControllerBase
{
    [HttpGet("excel")]
    public async Task<IActionResult> GetExcel(
        [FromQuery] int? agenceId,
        [FromQuery] DateOnly? dateDebut,
        [FromQuery] DateOnly? dateFin,
        [FromQuery] string? typeRapport)
    {
        var content = await reportService.GenerateSalesExcelAsync(agenceId, dateDebut, dateFin, typeRapport);
        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ventes.xlsx");
    }

    [HttpGet("pdf")]
    public async Task<IActionResult> GetPdf(
        [FromQuery] int? agenceId,
        [FromQuery] DateOnly? dateDebut,
        [FromQuery] DateOnly? dateFin,
        [FromQuery] string? typeRapport)
    {
        var content = await reportService.GenerateSalesPdfAsync(agenceId, dateDebut, dateFin, typeRapport);
        return File(content, "application/pdf", "ventes.pdf");
    }
}
