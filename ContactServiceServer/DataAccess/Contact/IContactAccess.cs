using ContactServiceServer.Grains.Contact;
using Marten;

namespace ContactServiceServer.DataAccess.Contact;

public interface IContactAccess
{
    Task SaveStateAsync(Guid contactId, ContactState contactState);
    Task<ContactState?> LoadStateAsync(Guid contactId);
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
    }

    public async Task<ContactState?> LoadStateAsync(Guid contactId)
    {
        await using var session = _documentStore.QuerySession();
        var contactEntry = await session.LoadAsync<ContactEntry>(contactId);
        return contactEntry?.Map();
    }
}

public static class ContactRegistrationExtension
{
    public static StoreOptions RegisterContactSchema(this StoreOptions options)
    {
        options.Schema
            .For<ContactEntry>()
            .DatabaseSchemaName("contact")
            .Index(entry => entry.OwnerTenantId);

        return options;
    }
}