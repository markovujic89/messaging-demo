using ConsumerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsumerApi.Repository;

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _db;

    public MessageRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Message?> GetByIdAsync(int id)
    {
        return await _db.Messages.FindAsync(id);
    }

    public async Task<Message?> GetByMessageIdAsync(Guid messageId)
    {
        return await _db.Messages
            .FirstOrDefaultAsync(m => m.MessageId == messageId);
    }

    public async Task<IEnumerable<Message>> GetAllAsync()
    {
        return await _db.Messages.ToListAsync();
    }

    public async Task AddAsync(Message message)
    {
        await _db.Messages.AddAsync(message);
    }

    public async Task<bool> ExistsAsync(Guid messageId)
    {
        return await _db.Messages
            .AnyAsync(m => m.MessageId == messageId);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
    
    public async Task<bool> AddIfNotExistsAsync(Message message)
    {
        bool exists = await ExistsAsync(message.MessageId);

        if (exists)
            return false;

        await _db.Messages.AddAsync(message);
        await _db.SaveChangesAsync();
        return true;
    }
}