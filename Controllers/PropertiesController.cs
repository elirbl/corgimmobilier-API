using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using YmmoApi.Common;
using YmmoApi.Dtos.Properties;
using YmmoApi.Models;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Controllers;

[ApiController]
[Route("api/properties")]
public class PropertiesController(
    IPropertyService propertyService,
    ICurrentUserService currentUser,
    IValidator<PropertyCreateDto> createValidator,
    IValidator<PropertyUpdateDto> updateValidator,
    IValidator<PropertyStatusUpdateDto> statusValidator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<PropertyListItemDto>>>> GetAll(
        [FromQuery] PropertyType? type,
        [FromQuery] PropertyStatus? status,
        [FromQuery] decimal? prixMin,
        [FromQuery] decimal? prixMax,
        [FromQuery] double? surface,
        [FromQuery] string? ville,
        [FromQuery] PropertyDpe? dpe,
        [FromQuery] int? agentId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sort = null)
    {
        var filter = new PropertyFilter
        {
            Type = type,
            Status = status,
            MinPrice = prixMin,
            MaxPrice = prixMax,
            MinArea = surface,
            City = ville,
            DpeRating = dpe,
            AgentId = agentId
        };

        var result = await propertyService.GetPagedAsync(filter, page, pageSize, sort, currentUser.IsAuthenticated);
        return Ok(ApiResponse<PagedResult<PropertyListItemDto>>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<PropertyDetailDto>>> GetById(int id)
    {
        var property = await propertyService.GetDetailAsync(id);
        if (property is null)
            return NotFound(ApiResponse<PropertyDetailDto>.Fail("Bien introuvable."));

        return Ok(ApiResponse<PropertyDetailDto>.Ok(property));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Agent")]
    public async Task<ActionResult<ApiResponse<PropertyDetailDto>>> Create(PropertyCreateDto dto)
    {
        var validation = await createValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return ValidationProblem(BuildModelState(validation));

        var created = await propertyService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, ApiResponse<PropertyDetailDto>.Ok(created));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,Agent")]
    public async Task<ActionResult<ApiResponse<object>>> Update(int id, PropertyUpdateDto dto)
    {
        var validation = await updateValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return ValidationProblem(BuildModelState(validation));

        var updated = await propertyService.UpdateAsync(id, dto);
        if (!updated)
            return NotFound(ApiResponse<object>.Fail("Bien introuvable."));

        return NoContent();
    }

    [HttpPatch("{id:int}/statut")]
    [Authorize(Roles = "Admin,Agent")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateStatus(int id, PropertyStatusUpdateDto dto)
    {
        var validation = await statusValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return ValidationProblem(BuildModelState(validation));

        var result = await propertyService.UpdateStatusAsync(id, dto);
        if (!result.Succeeded)
            return NotFound(ApiResponse<object>.Fail(result.Error!));

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin,Agent")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var deleted = await propertyService.DeleteAsync(id);
        if (!deleted)
            return NotFound(ApiResponse<object>.Fail("Bien introuvable."));

        return NoContent();
    }

    [HttpPost("{id:int}/photos")]
    [Authorize(Roles = "Admin,Agent")]
    [RequestSizeLimit(60_000_000)]
    public async Task<ActionResult<ApiResponse<PropertyPhotoUploadResultDto>>> AddPhotos(int id, [FromForm] List<IFormFile> files)
    {
        var result = await propertyService.AddPhotosAsync(id, files);
        if (!result.Succeeded)
            return NotFound(ApiResponse<PropertyPhotoUploadResultDto>.Fail(result.Error!));

        return Ok(ApiResponse<PropertyPhotoUploadResultDto>.Ok(result.Data!));
    }

    [HttpDelete("{id:int}/photos/{photoId:int}")]
    [Authorize(Roles = "Admin,Agent")]
    public async Task<ActionResult<ApiResponse<object>>> DeletePhoto(int id, int photoId)
    {
        var result = await propertyService.DeletePhotoAsync(id, photoId);
        if (!result.Succeeded)
            return NotFound(ApiResponse<object>.Fail(result.Error!));

        return NoContent();
    }

    [HttpGet("cities")]
    public async Task<ActionResult<ApiResponse<IEnumerable<string>>>> GetCities()
    {
        var result = await propertyService.GetPagedAsync(new PropertyFilter(), 1, 1000, null, true);
        var cities = result.Items.Select(p => p.City).Distinct().OrderBy(c => c);
        return Ok(ApiResponse<IEnumerable<string>>.Ok(cities));
    }

    private static ModelStateDictionary BuildModelState(FluentValidation.Results.ValidationResult validation)
    {
        var modelState = new ModelStateDictionary();
        foreach (var error in validation.Errors)
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);

        return modelState;
    }
}
