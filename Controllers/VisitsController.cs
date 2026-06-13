using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using YmmoApi.Common;
using YmmoApi.Dtos.Visits;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Controllers;

[ApiController]
[Route("api/visits")]
[Authorize]
public class VisitsController(
    IVisitService visitService,
    ICurrentUserService currentUser,
    IValidator<VisitCreateDto> createValidator,
    IValidator<VisitStatusUpdateDto> statusValidator) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Client")]
    public async Task<ActionResult<ApiResponse<VisitResponseDto>>> Create(VisitCreateDto dto)
    {
        var validation = await createValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return ValidationProblem(BuildModelState(validation));

        var result = await visitService.CreateAsync(dto, currentUser.UserId!.Value);
        if (!result.Succeeded)
            return BadRequest(ApiResponse<VisitResponseDto>.Fail(result.Error!));

        return Ok(ApiResponse<VisitResponseDto>.Ok(result.Data!));
    }

    [HttpGet("agent/{agentId:int}")]
    [Authorize(Roles = "Admin,Agent")]
    public async Task<ActionResult<ApiResponse<List<VisitResponseDto>>>> GetAgentCalendar(int agentId, [FromQuery] DateOnly? date)
    {
        var result = await visitService.GetAgentCalendarAsync(agentId, date);
        if (!result.Succeeded)
            return NotFound(ApiResponse<List<VisitResponseDto>>.Fail(result.Error!));

        return Ok(ApiResponse<List<VisitResponseDto>>.Ok(result.Data!));
    }

    [HttpPatch("{id:int}/statut")]
    [Authorize(Roles = "Admin,Agent")]
    public async Task<ActionResult<ApiResponse<VisitResponseDto>>> UpdateStatus(int id, VisitStatusUpdateDto dto)
    {
        var validation = await statusValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return ValidationProblem(BuildModelState(validation));

        var result = await visitService.UpdateStatusAsync(id, dto);
        if (!result.Succeeded)
            return BadRequest(ApiResponse<VisitResponseDto>.Fail(result.Error!));

        return Ok(ApiResponse<VisitResponseDto>.Ok(result.Data!));
    }

    [HttpGet("disponibilites")]
    public async Task<ActionResult<ApiResponse<List<TimeSlotDto>>>> GetAvailability([FromQuery] int agentId, [FromQuery] DateOnly date)
    {
        var result = await visitService.GetAvailabilityAsync(agentId, date);
        if (!result.Succeeded)
            return NotFound(ApiResponse<List<TimeSlotDto>>.Fail(result.Error!));

        return Ok(ApiResponse<List<TimeSlotDto>>.Ok(result.Data!));
    }

    private static ModelStateDictionary BuildModelState(FluentValidation.Results.ValidationResult validation)
    {
        var modelState = new ModelStateDictionary();
        foreach (var error in validation.Errors)
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);

        return modelState;
    }
}
