using YmmoApi.Dtos.Auth;

namespace YmmoApi.Services.Interfaces;

public class AuthResult
{
    public bool Succeeded { get; init; }
    public string? Error { get; init; }
    public TokenResponseDto? Tokens { get; init; }
    public UserProfileDto? User { get; init; }

    public static AuthResult Success(TokenResponseDto tokens, UserProfileDto user) =>
        new() { Succeeded = true, Tokens = tokens, User = user };

    public static AuthResult Failure(string error) =>
        new() { Succeeded = false, Error = error };
}

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(RegisterDto dto);
    Task<AuthResult> LoginAsync(LoginDto dto);
    Task<AuthResult> RefreshAsync(string refreshToken);
    Task<bool> LogoutAsync(string refreshToken);
    Task ForgotPasswordAsync(string email);
}
