using ContactServiceServer.Grains.Contact;
using Marten;

namespace ContactServiceServer.DataAccess.Contact;

public interface IContactAccess
{
    Task SaveStateAsync(Guid contactId, ContactState contactState);
    Task<ContactState?> LoadStateAsync(Guid contactId);

    Task<IEnumerable<TenantContactRelation>> ListTenantContactsAsync(Guid tenantId);
}

public class ContactAccess : IContactAccess
{
    private readonly IDocumentStore _documentStore;
    
    public ContactAccess(IDocumentStore documentStore)
    {
        _documentStore = documentStore;
    }

    public async Task SaveStateAsync(Guid contactId, ContactState contactState)
    {
        await using var session = _documentStore.LightweightSession();
        session.Store(contactState.Map(contactId));
        await session.SaveChangesAsync();
    }

    public async Task<ContactState?> LoadStateAsync(Guid contactId)
    {
        await using var session = _documentStore.QuerySession();
        var contactEntry = await session.LoadAsync<ContactEntry>(contactId);
        return contactEntry?.Map();
    }

    public async Task<IEnumerable<TenantContactRelation>> ListTenantContactsAsync(Guid tenantId)
    {
        await using var session = _documentStore.QuerySession();
        var contacts = await session.Query<ContactEntry>()
            .Where(entry => entry.OwnerTenantId == tenantId)
            .ToListAsync();

        return contacts.Select(entry => new TenantContactRelation(tenantId, entry.Id));
    }
}

public static class ContactRegistrationExtension
{
    public static StoreOptions RegisterContactSchema(this StoreOptions options)
    {
        options.Schema
            .For<ContactEntry>()
            .Index(entry => entry.OwnerTenantId);

        return options;
    }
}