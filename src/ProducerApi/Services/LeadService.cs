using Lead.Contracts;
using MassTransit;

namespace ProducerApi.Services;

public class LeadService : ILeadService
{
    private readonly IPublishEndpoint _publishEndpoint;

    public LeadService(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishLead(CreateLeadRequest request)
    {
        var message = new LeadCreated(
            LeadId: Guid.NewGuid(),
            AgencyId: request.AgencyId,
            Name: request.Name,
            Email: request.Email,
            CreatedAt: DateTime.UtcNow
        );

        // publish — MassTransit will map to exchange and routing
        await _publishEndpoint.Publish(message);
    }
}