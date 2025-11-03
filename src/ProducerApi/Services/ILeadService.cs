using Lead.Contracts;

namespace ProducerApi.Services;

public interface ILeadService
{
    Task PublishLead(CreateLeadRequest request);
}