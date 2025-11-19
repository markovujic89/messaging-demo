using Microsoft.EntityFrameworkCore;

namespace ProducerApi.Models;

public class ProducerDbContext : DbContext
{
    public ProducerDbContext(DbContextOptions<ProducerDbContext> options)
    : base(options) { }
    
    public DbSet<Lead> Leads => Set<Lead>();
    
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>(); // <--- NEW>
}