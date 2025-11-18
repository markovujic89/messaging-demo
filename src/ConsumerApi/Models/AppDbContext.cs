using Microsoft.EntityFrameworkCore;

namespace ConsumerApi.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
    
    public DbSet<Message> Messages => Set<Message>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Message>(b =>
        {
            b.ToTable("Messages");
            b.HasKey(x => x.Id);
            b.Property(x => x.LeadId).IsRequired();
            b.HasIndex(x => x.MessageId).IsUnique(); // unique constraint on MessageId
        });
    }
}