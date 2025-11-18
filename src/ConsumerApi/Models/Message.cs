namespace ConsumerApi.Models;

public class Message
{
    public int Id { get; set; }
    
    public Guid MessageId { get; set; }
    
    public Guid LeadId { get; set; }
}