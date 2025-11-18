using ConsumerApi.Models;

namespace ConsumerApi.Repository;

public interface IMessageRepository
{
    Task<Message?> GetByIdAsync(int id);
    Task<Message?> GetByMessageIdAsync(Guid messageId);
    Task<IEnumerable<Message>> GetAllAsync();
    Task AddAsync(Message message);
    Task<bool> ExistsAsync(Guid messageId);
    Task SaveChangesAsync();

    Task<bool> AddIfNotExistsAsync(Message message);
}