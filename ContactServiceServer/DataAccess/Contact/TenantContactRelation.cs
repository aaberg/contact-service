namespace ContactServiceServer.DataAccess.Contact;

public record TenantContactRelation(Guid TenantId, Guid ContactId);