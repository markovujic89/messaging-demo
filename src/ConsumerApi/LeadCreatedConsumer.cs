using ConsumerApi.Models;
using ConsumerApi.Repository;
using Lead.Contracts;
using MassTransit;

namespace ConsumerApi;

public class LeadCreatedConsumer : IConsumer<LeadCreated>
{
    private readonly IMessageRepository _messageRepository;

    public LeadCreatedConsumer(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task Consume(ConsumeContext<LeadCreated> context)
    {
        var msg = context.Message;
        
        await _messageRepository.AddIfNotExistsAsync(new Message
        {
            LeadId = context.Message.LeadId,
            MessageId = context.Message.MessageId
        });
    }
}