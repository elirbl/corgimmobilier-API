using System.Security.Claims;
using YmmoApi.Models;
using YmmoApi.Services;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Middleware;

/// <summary>
/// S'exécute après l'authentification JWT et alimente <see cref="ICurrentUserService"/>
/// (scoped) à partir des claims du token, pour un accès simple dans les services/contrôleurs.
/// </summary>
public class CurrentUserMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ICurrentUserService currentUser)
    {
        if (context.User.Identity?.IsAuthenticated == true && currentUser is CurrentUserService service)
        {
            service.IsAuthenticated = true;
            service.Email = context.User.FindFirstValue(ClaimTypes.Email);

            var idClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(idClaim, out var userId))
                service.UserId = userId;

            var roleClaim = context.User.FindFirstValue(ClaimTypes.Role);
            if (Enum.TryParse<UserRole>(roleClaim, out var role))
                service.Role = role;
        }

        await next(context);
    }
}
