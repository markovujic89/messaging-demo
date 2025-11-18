using Lead.Contracts;
using MassTransit;

namespace ProducerApi.Services;

public class LeadService : ILeadService
{
    private readonly IPublishEndpoint _publishEndpoint;
    private const int BulkPublishCount = 100000;

    public LeadService(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishLead(CreateLeadRequest request)
    {
        var message = new LeadCreated(
            MessageId: Guid.NewGuid(),
            LeadId: Guid.NewGuid(),
            AgencyId: request.AgencyId,
            Name: request.Name,
            Email: request.Email,
            CreatedAt: DateTime.UtcNow
        );

        // publish — MassTransit will map to exchange and routing
        await _publishEndpoint.Publish(message);
    }

    public async Task BulkPublish()
    {
        var tasks = new List<Task>();

        for (var i = 0; i < BulkPublishCount; i++)
        {
            tasks.Add(_publishEndpoint.Publish(CreateNewLead()));
        }

        await Task.WhenAll(tasks);
    }

    private LeadCreated CreateNewLead()
    {
        return new LeadCreated(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), $"name_{new Random().Next(1,5000)}", $"test@test{new Random().Next(1,5000)}", DateTime.UtcNow);
    }
}