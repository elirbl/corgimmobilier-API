namespace YmmoApi.Dtos.Properties;

public class PropertyPhotoUploadResultDto
{
    public List<PhotoDto> Added { get; set; } = [];
    public List<string> Errors { get; set; } = [];
}
