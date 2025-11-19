namespace ProducerApi.Models;

public class OutboxMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Type { get; set; } // "LeadCreated", etc.
    public string Data { get; set; } // JSON serialized message
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }
    public int RetryCount { get; set; } = 0;
    public string? Error { get; set; }
}