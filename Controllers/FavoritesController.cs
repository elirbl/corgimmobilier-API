using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YmmoApi.Common;
using YmmoApi.Dtos.Favorites;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Controllers;

[ApiController]
[Route("api/favorites")]
[Authorize(Roles = "Client")]
public class FavoritesController(IFavoriteService favoriteService, ICurrentUserService currentUser) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<FavoriteDto>>>> GetAll()
    {
        var favorites = await favoriteService.GetByUserAsync(currentUser.UserId!.Value);
        return Ok(ApiResponse<List<FavoriteDto>>.Ok(favorites));
    }

    [HttpPost("{propertyId:int}")]
    public async Task<ActionResult<ApiResponse<FavoriteDto>>> Add(int propertyId)
    {
        var result = await favoriteService.AddAsync(currentUser.UserId!.Value, propertyId);
        if (!result.Succeeded)
            return NotFound(ApiResponse<FavoriteDto>.Fail(result.Error!));

        return Ok(ApiResponse<FavoriteDto>.Ok(result.Data!));
    }

    [HttpDelete("{propertyId:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Remove(int propertyId)
    {
        var result = await favoriteService.RemoveAsync(currentUser.UserId!.Value, propertyId);
        if (!result.Succeeded)
            return NotFound(ApiResponse<object>.Fail(result.Error!));

        return Ok(ApiResponse<object>.Ok(new { }));
    }
}
