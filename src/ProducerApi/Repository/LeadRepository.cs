using System.Text.Json;
using Lead.Contracts;
using MassTransit;
using MassTransit.Testing;
using ProducerApi.Models;

namespace ProducerApi.Repository;

public class LeadRepository : ILeadRepository
{
    private readonly ProducerDbContext _context;
    
    private readonly IPublishEndpoint _publishEndpoint;

    public LeadRepository(ProducerDbContext context, 
        IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
    }

    public async Task InsertAndPublishLead(LeadCreated lead)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.Leads.AddAsync(new Models.Lead
            {
                AgencyId = lead.AgencyId,
                CreatedAt = lead.CreatedAt,
                Email = lead.Email,
                Name = lead.Name
            });
        
            var outboxMessage = new OutboxMessage
            {
                Type = lead.GetType().Name,
                Data = JsonSerializer.Serialize(lead, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }),
                CreatedAt = DateTime.UtcNow
            };

            await _context.OutboxMessages.AddAsync(outboxMessage);
            await _context.SaveChangesAsync();
            
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
        }
        
    }
}