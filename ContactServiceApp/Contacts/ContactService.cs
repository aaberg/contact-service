using System.Buffers;
using System.Collections.Immutable;
using ContactServiceGrainInterfaces.User;

namespace ContactServiceApp.Contacts;

public class ContactService
{
    private readonly ILogger<ContactService> _logger;
    private readonly IClusterClient _client;

    public ContactService(ILogger<ContactService> logger, IClusterClient client)
    {
        _logger = logger;
        _client = client;
    }

    public async Task<ImmutableArray<Contact>> GetAllContacts(string userId)
    {
        var userGrain = _client.GetGrain<IUserGrain>(userId);

        var tenantGrain = await userGrain.GetSelectedTenant();

        var contactGrains = await tenantGrain.ListContacts();

        var tasks = ArrayPool<Task<string>>.Shared.Rent(contactGrains.Length);
        for (var i = 0; i < contactGrains.Length; i++)
        {
            tasks[i] = contactGrains[i].GetName();
        }

        var contacts = ImmutableArray.CreateBuilder<Contact>(tasks.Length);

        for (int i = 0; i < contactGrains.Length; i++)
        {
            var name = await tasks[i];
            
            contacts.Add(new Contact(name));
        }

        return contacts.ToImmutable();
    }
}