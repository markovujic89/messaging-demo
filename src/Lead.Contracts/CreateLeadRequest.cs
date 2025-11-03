namespace Lead.Contracts;

public class CreateLeadRequest
{
    public Guid AgencyId { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
}