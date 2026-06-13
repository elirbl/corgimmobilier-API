using YmmoApi.Models;

namespace YmmoApi.Dtos.Visits;

public class VisitResponseDto
{
    public int Id { get; set; }
    public int PropertyId { get; set; }
    public string PropertyTitle { get; set; } = string.Empty;
    public int ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public int? AgentId { get; set; }
    public string? AgentName { get; set; }
    public DateTime ScheduledAt { get; set; }
    public VisitStatus Status { get; set; }
    public string Notes { get; set; } = string.Empty;
}
