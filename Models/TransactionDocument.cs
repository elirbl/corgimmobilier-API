namespace YmmoApi.Models;

public class TransactionDocument
{
    public int Id { get; set; }
    public int TransactionId { get; set; }
    public Transaction? Transaction { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
