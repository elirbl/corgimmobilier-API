namespace YmmoApi.Dtos.Visits;

public class VisitCreateDto
{
    public int PropertyId { get; set; }
    public int? AgentId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string? Notes { get; set; }
}
