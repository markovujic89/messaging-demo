namespace ProducerApi.Models;

public class Lead
{
    public int Id { get; set; }
    
    public Guid AgencyId { get; set; }
    
    public string Name { get; set; } = default!;
    
    public string Email { get; set; } = default!;
    
    public DateTime CreatedAt { get; set; }
}