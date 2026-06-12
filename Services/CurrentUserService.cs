using System.Security.Claims;
using YmmoApi.Models;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Services;

public class CurrentUserService : ICurrentUserService
{
    public int? UserId { get; set; }
    public string? Email { get; set; }
    public UserRole? Role { get; set; }
    public bool IsAuthenticated { get; set; }
}
