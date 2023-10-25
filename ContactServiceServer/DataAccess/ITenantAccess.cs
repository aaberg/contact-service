using ContactServiceServer.DataAccess.Models;
using Marten;
using Microsoft.Extensions.DependencyInjection;

namespace ContactServiceServer.DataAccess;

public interface ITenantAccess
{
    public Task<IEnumerable<TenantUserRelationEntry>> GetTenantRelations(Guid tenantId);

    public Task<IEnumerable<TenantUserRelationEntry>> GetUserRelations(string userId);

    public Task GiveUserAccessToTenant(Guid tenantId, string userId);
}

public class TenantAccess : ITenantAccess
{
    private readonly IDocumentStore _documentStore;

    public TenantAccess(IDocumentStore documentStore)
    {
        _documentStore = documentStore;
    }

    public async Task<IEnumerable<TenantUserRelationEntry>> GetTenantRelations(Guid tenantId)
    {
        await using var session = _documentStore.LightweightSession();
        return await session
            .Query<TenantUserRelationEntry>()
            .Where(relation => relation.TenantId == tenantId)
            .ToListAsync();
    }

    public async Task<IEnumerable<TenantUserRelationEntry>> GetUserRelations(string userId)
    {
        await using var session = _documentStore.LightweightSession();
        return await session
            .Query<TenantUserRelationEntry>()
            .Where(relation => relation.UserId == userId)
            .ToListAsync();
    }

    public async Task GiveUserAccessToTenant(Guid tenantId, string userId)
    {
        var relation = new TenantUserRelationEntry(tenantId, userId);
        
        await using var session = _documentStore.LightweightSession();
        
        session.Store(relation);

        await session.SaveChangesAsync();
    }
}

public static class TenantAccessRegistrationExtension
{
    public static StoreOptions RegisterTenantAccessSchema(this StoreOptions options)
    {
        options.Schema
            .For<TenantUserRelationEntry>()
            .DatabaseSchemaName("tenantuserrelation")
            .Index(relation => relation.TenantId)
            .Index(relation => relation.UserId);

        return options;
    }
}