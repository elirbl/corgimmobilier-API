namespace YmmoApi.Models;

public class Photo
{
    public int Id { get; set; }
    public int PropertyId { get; set; }
    public Property? Property { get; set; }
    public string Url { get; set; } = string.Empty;
    public bool IsMain { get; set; }
}
