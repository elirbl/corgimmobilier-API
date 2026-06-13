using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using YmmoApi.Common;
using YmmoApi.Dtos.Transactions;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Controllers;

[ApiController]
[Route("api/transactions")]
[Authorize]
public class TransactionsController(
    ITransactionService transactionService,
    ICurrentUserService currentUser,
    IValidator<TransactionCreateDto> createValidator,
    IValidator<TransactionStageUpdateDto> stageValidator) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Client")]
    public async Task<ActionResult<ApiResponse<TransactionDetailDto>>> Create(TransactionCreateDto dto)
    {
        var validation = await createValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return ValidationProblem(BuildModelState(validation));

        var result = await transactionService.CreateAsync(dto, currentUser.UserId!.Value);
        if (!result.Succeeded)
            return BadRequest(ApiResponse<TransactionDetailDto>.Fail(result.Error!));

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, ApiResponse<TransactionDetailDto>.Ok(result.Data));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<TransactionDetailDto>>> GetById(int id)
    {
        var result = await transactionService.GetDetailAsync(id, currentUser.UserId!.Value, currentUser.Role!.Value);
        if (!result.Succeeded)
            return result.IsForbidden
                ? Forbid()
                : NotFound(ApiResponse<TransactionDetailDto>.Fail(result.Error!));

        return Ok(ApiResponse<TransactionDetailDto>.Ok(result.Data!));
    }

    [HttpPatch("{id:int}/etape")]
    [Authorize(Roles = "Admin,Agent")]
    public async Task<ActionResult<ApiResponse<TransactionDetailDto>>> AdvanceStage(int id, TransactionStageUpdateDto dto)
    {
        var validation = await stageValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return ValidationProblem(BuildModelState(validation));

        var result = await transactionService.AdvanceStageAsync(id, dto, currentUser.UserId!.Value);
        if (!result.Succeeded)
            return NotFound(ApiResponse<TransactionDetailDto>.Fail(result.Error!));

        return Ok(ApiResponse<TransactionDetailDto>.Ok(result.Data!));
    }

    [HttpPost("{id:int}/documents")]
    [RequestSizeLimit(15_000_000)]
    public async Task<ActionResult<ApiResponse<TransactionDocumentDto>>> AddDocument(int id, IFormFile file)
    {
        var result = await transactionService.AddDocumentAsync(id, file, currentUser.UserId!.Value, currentUser.Role!.Value);
        if (!result.Succeeded)
            return result.IsForbidden
                ? Forbid()
                : BadRequest(ApiResponse<TransactionDocumentDto>.Fail(result.Error!));

        return Ok(ApiResponse<TransactionDocumentDto>.Ok(result.Data!));
    }

    private static ModelStateDictionary BuildModelState(FluentValidation.Results.ValidationResult validation)
    {
        var modelState = new ModelStateDictionary();
        foreach (var error in validation.Errors)
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);

        return modelState;
    }
}
