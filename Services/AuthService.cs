using YmmoApi.Dtos.Auth;
using YmmoApi.Models;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Services;

public class AuthService(IUserRepository userRepository, ITokenService tokenService) : IAuthService
{
    public async Task<AuthResult> RegisterAsync(RegisterDto dto)
    {
        if (await userRepository.GetByEmailAsync(dto.Email) is not null)
            return AuthResult.Failure("Un compte existe déjà avec cet email.");

        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone,
            Role = dto.Role,
            AgencyId = dto.AgencyId,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        await userRepository.AddAsync(user);
        await userRepository.SaveChangesAsync();

        return await IssueTokensAsync(user);
    }

    public async Task<AuthResult> LoginAsync(LoginDto dto)
    {
        var user = await userRepository.GetByEmailAsync(dto.Email);
        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return AuthResult.Failure("Email ou mot de passe invalide.");

        return await IssueTokensAsync(user);
    }

    public async Task<AuthResult> RefreshAsync(string refreshToken)
    {
        var existingToken = await userRepository.GetRefreshTokenAsync(refreshToken);
        if (existingToken is null || !existingToken.IsActive || existingToken.User is null)
            return AuthResult.Failure("Refresh token invalide ou expiré.");

        existingToken.RevokedAt = DateTime.UtcNow;

        return await IssueTokensAsync(existingToken.User);
    }

    public async Task<bool> LogoutAsync(string refreshToken)
    {
        var existingToken = await userRepository.GetRefreshTokenAsync(refreshToken);
        if (existingToken is null || !existingToken.IsActive)
            return false;

        existingToken.RevokedAt = DateTime.UtcNow;
        await userRepository.SaveChangesAsync();
        return true;
    }

    public async Task ForgotPasswordAsync(string email)
    {
        // Ne révèle jamais si l'email existe ou non.
        // L'envoi d'email de réinitialisation sera branché sur un futur service de mailing.
        await Task.CompletedTask;
    }

    private async Task<AuthResult> IssueTokensAsync(User user)
    {
        var accessToken = tokenService.GenerateAccessToken(user);
        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = tokenService.GenerateRefreshToken(),
            ExpiresAt = tokenService.GetRefreshTokenExpiry()
        };

        await userRepository.AddRefreshTokenAsync(refreshToken);
        await userRepository.SaveChangesAsync();

        var tokens = new TokenResponseDto
        {
            AccessToken = accessToken,
            AccessTokenExpiresAt = tokenService.GetAccessTokenExpiry(),
            RefreshToken = refreshToken.Token,
            RefreshTokenExpiresAt = refreshToken.ExpiresAt
        };

        var profile = new UserProfileDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone,
            Role = user.Role,
            AgencyId = user.AgencyId
        };

        return AuthResult.Success(tokens, profile);
    }
}
