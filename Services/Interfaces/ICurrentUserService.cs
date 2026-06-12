using YmmoApi.Models;

namespace YmmoApi.Services.Interfaces;

public interface ICurrentUserService
{
    int? UserId { get; }
    string? Email { get; }
    UserRole? Role { get; }
    bool IsAuthenticated { get; }
}
