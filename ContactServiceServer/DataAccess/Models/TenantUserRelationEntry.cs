namespace ContactServiceServer.DataAccess.Models;

public record TenantUserRelationEntry(Guid TenantId, string UserId)
{
    public string Id => $"{TenantId}_{UserId}";
}