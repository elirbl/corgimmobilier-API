namespace YmmoApi.Models;

public class Visit
{
    public int Id { get; set; }
    public int PropertyId { get; set; }
    public Property? Property { get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    public int? AgentId { get; set; }
    public User? Agent { get; set; }
    public DateTime ScheduledAt { get; set; }
    public VisitStatus Status { get; set; } = VisitStatus.Scheduled;
    public string Notes { get; set; } = string.Empty;
}
