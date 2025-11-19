using Lead.Contracts;

namespace ProducerApi.Repository;

public interface ILeadRepository
{
    Task InsertAndPublishLead(LeadCreated leadCreated);
}