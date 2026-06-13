using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using YmmoApi.Common;
using YmmoApi.Dtos.Agencies;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Controllers;

[ApiController]
[Route("api/agencies")]
public class AgenciesController(
    IAgencyService agencyService,
    IValidator<AgencyCreateDto> createValidator,
    IValidator<AgencyUpdateDto> updateValidator,
    IValidator<AttachAgentDto> attachAgentValidator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<AgencyResponseDto>>>> GetAll(
        [FromQuery] string? city,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var result = await agencyService.GetPagedAsync(city, page, pageSize);
        return Ok(ApiResponse<PagedResult<AgencyResponseDto>>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<AgencyDetailDto>>> GetById(int id)
    {
        var agency = await agencyService.GetDetailAsync(id);
        if (agency is null)
            return NotFound(ApiResponse<AgencyDetailDto>.Fail("Agence introuvable."));

        return Ok(ApiResponse<AgencyDetailDto>.Ok(agency));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<AgencyResponseDto>>> Create(AgencyCreateDto dto)
    {
        var validation = await createValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return ValidationProblem(BuildModelState(validation));

        var created = await agencyService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, ApiResponse<AgencyResponseDto>.Ok(created));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<object>>> Update(int id, AgencyUpdateDto dto)
    {
        var validation = await updateValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return ValidationProblem(BuildModelState(validation));

        var updated = await agencyService.UpdateAsync(id, dto);
        if (!updated)
            return NotFound(ApiResponse<object>.Fail("Agence introuvable."));

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var deleted = await agencyService.DeleteAsync(id);
        if (!deleted)
            return NotFound(ApiResponse<object>.Fail("Agence introuvable."));

        return NoContent();
    }

    [HttpPost("{id:int}/agents")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<object>>> AttachAgent(int id, AttachAgentDto dto)
    {
        var validation = await attachAgentValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return ValidationProblem(BuildModelState(validation));

        var result = await agencyService.AttachAgentAsync(id, dto.AgentId);
        if (!result.Succeeded)
            return BadRequest(ApiResponse<object>.Fail(result.Error!));

        return Ok(ApiResponse<object>.Ok(new { }, "Agent rattaché avec succès."));
    }

    private static ModelStateDictionary BuildModelState(FluentValidation.Results.ValidationResult validation)
    {
        var modelState = new ModelStateDictionary();
        foreach (var error in validation.Errors)
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);

        return modelState;
    }
}
