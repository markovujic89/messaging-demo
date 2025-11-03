using Lead.Contracts;
using MassTransit;

namespace ConsumerApi;

public class LeadCreatedConsumer : IConsumer<LeadCreated>
{
    public async Task Consume(ConsumeContext<LeadCreated> context)
    {
        var msg = context.Message;
        
        return;
    }
}