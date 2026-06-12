using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using YmmoApi.Dtos.Auth;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
    IAuthService authService,
    IValidator<RegisterDto> registerValidator,
    IValidator<LoginDto> loginValidator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<TokenResponseDto>> Register(RegisterDto dto)
    {
        var validation = await registerValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return ValidationProblem(BuildValidationProblem(validation));

        var result = await authService.RegisterAsync(dto);
        if (!result.Succeeded)
            return Conflict(new { error = result.Error });

        return Ok(result.Tokens);
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDto>> Login(LoginDto dto)
    {
        var validation = await loginValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return ValidationProblem(BuildValidationProblem(validation));

        var result = await authService.LoginAsync(dto);
        if (!result.Succeeded)
            return Unauthorized(new { error = result.Error });

        return Ok(result.Tokens);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<TokenResponseDto>> Refresh(RefreshTokenRequestDto dto)
    {
        var result = await authService.RefreshAsync(dto.RefreshToken);
        if (!result.Succeeded)
            return Unauthorized(new { error = result.Error });

        return Ok(result.Tokens);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(RefreshTokenRequestDto dto)
    {
        var revoked = await authService.LogoutAsync(dto.RefreshToken);
        return revoked ? NoContent() : NotFound();
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
    {
        await authService.ForgotPasswordAsync(dto.Email);
        // Réponse générique pour ne pas révéler si l'email existe
        return Ok(new { message = "Si un compte existe pour cet email, un lien de réinitialisation a été envoyé." });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserProfileDto>> Me([FromServices] IUserRepository userRepository, [FromServices] ICurrentUserService currentUser)
    {
        if (currentUser.UserId is not { } userId)
            return Unauthorized();

        var user = await userRepository.GetByIdAsync(userId);
        if (user is null)
            return NotFound();

        return Ok(new UserProfileDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone,
            Role = user.Role,
            AgencyId = user.AgencyId
        });
    }

    private static ModelStateDictionary BuildValidationProblem(FluentValidation.Results.ValidationResult validation)
    {
        var modelState = new ModelStateDictionary();
        foreach (var error in validation.Errors)
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);

        return modelState;
    }
}
