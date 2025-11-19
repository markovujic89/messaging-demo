using System.Text.Json;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProducerApi.Models;

namespace ProducerApi.BackgroundTasks;

public class OutboxProcessor : IOutboxProcessor
{
    private readonly ProducerDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<OutboxProcessor> _logger;

    public OutboxProcessor(ProducerDbContext context, IPublishEndpoint publishEndpoint, ILogger<OutboxProcessor> logger)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task ProcessOutboxMessagesAsync()
    {
        var messages = await _context.OutboxMessages
            .Where(m => m.ProcessedAt == null)
            .OrderBy(m => m.CreatedAt)
            .Take(50) // Process in batches
            .ToListAsync();

        foreach (var message in messages)
        {
            try
            {
                // Deserialize message
                var messageType = Type.GetType($"YourNamespace.Events.{message.Type}, YourAssembly");
                if (messageType == null)
                {
                    _logger.LogWarning("Unknown message type: {MessageType}", message.Type);
                    continue;
                }

                var eventMessage = JsonSerializer.Deserialize(message.Data, messageType);

                // Publish to message broker
                await _publishEndpoint.Publish(eventMessage, eventMessage.GetType());

                // Mark as processed
                message.ProcessedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully processed outbox message {MessageId}", message.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process outbox message {MessageId}", message.Id);
                
                // Update retry count
                message.RetryCount++;
                message.Error = ex.Message;

                // If too many retries, mark as failed (opciono)
                if (message.RetryCount >= 5)
                {
                    message.ProcessedAt = DateTime.UtcNow; // Mark as processed but failed
                    _logger.LogWarning("Message {MessageId} exceeded retry limit", message.Id);
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}