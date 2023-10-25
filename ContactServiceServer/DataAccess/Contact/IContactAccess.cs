using ContactServiceServer.Grains.Contact;
using Marten;

namespace ContactServiceServer.DataAccess.Contact;

public interface IContactAccess
{
    Task SaveState(Guid contactId, ContactState contactState);
    Task<ContactState?> LoadState(Guid contactId);
}

public class ContactAccess : IContactAccess
{
    private readonly IDocumentStore _documentStore;

    public async Task SaveState(Guid contactId, ContactState contactState)
    {
        await using var session = _documentStore.LightweightSession();
        session.Store(contactState.Map(contactId));
    }

    public async Task<ContactState?> LoadState(Guid contactId)
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
            .DatabaseSchemaName("contact");

        return options;
    }
}