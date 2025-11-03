namespace Lead.Contracts;

public record LeadCreated(Guid LeadId, Guid AgencyId, string Name, string Email, DateTime CreatedAt);