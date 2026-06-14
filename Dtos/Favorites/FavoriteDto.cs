using YmmoApi.Dtos.Properties;

namespace YmmoApi.Dtos.Favorites;

public class FavoriteDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public PropertyListItemDto Property { get; set; } = new();
}
