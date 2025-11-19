using Lead.Contracts;
using MassTransit;
using ProducerApi.Repository;

namespace ProducerApi.Services;

public class LeadService : ILeadService
{
    private readonly IPublishEndpoint _publishEndpoint;
    private const int BulkPublishCount = 4000;
    private readonly ILeadRepository _leadRepository;

    public LeadService(IPublishEndpoint publishEndpoint, 
        ILeadRepository leadRepository)
    {
        _publishEndpoint = publishEndpoint;
        _leadRepository = leadRepository;
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
        
        await _leadRepository.InsertAndPublishLead(message);
    }

    public async Task BulkPublish()
    {
        var tasks = new List<Task>();

        for (var i = 0; i < BulkPublishCount; i++)
        {
            tasks.Add(_leadRepository.InsertAndPublishLead(CreateNewLead()));
        }

        await Task.WhenAll(tasks);
    }

    private LeadCreated CreateNewLead()
    {
        return new LeadCreated(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), $"name_{new Random().Next(1,5000)}", $"test@test{new Random().Next(1,5000)}", DateTime.UtcNow);
    }
}