using System.Buffers;
using System.Collections.Immutable;
using ContactServiceGrainInterfaces.Contact;
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

        var contactIds = ArrayPool<string>.Shared.Rent(contactGrains.Length);
        var nameTasks = ArrayPool<Task<string>>.Shared.Rent(contactGrains.Length);
        var phoneTasks = ArrayPool<Task<Phone[]>>.Shared.Rent(contactGrains.Length);
        var emailTasks = ArrayPool<Task<EmailAddress[]>>.Shared.Rent(contactGrains.Length);
        for (var i = 0; i < contactGrains.Length; i++)
        {
            contactIds[i] = contactGrains[i].GetGrainId().Key.ToString();
            nameTasks[i] = contactGrains[i].GetName();
            phoneTasks[i] = contactGrains[i].ListPhones();
            emailTasks[i] = contactGrains[i].ListEmails();
        }

        var contacts = ImmutableArray.CreateBuilder<Contact>(nameTasks.Length);

        for (int i = 0; i < contactGrains.Length; i++)
        {
            var name = await nameTasks[i];
            var phones = await phoneTasks[i];
            var emails = await emailTasks[i];
            
            contacts.Add(new Contact(contactIds[i], name, emails, phones));
        }

        return contacts.ToImmutable();
    }
}