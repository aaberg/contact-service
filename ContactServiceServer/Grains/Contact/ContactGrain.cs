using ContactServiceGrainInterfaces.Contact;
using ContactServiceServer.DataAccess.Contact;

namespace ContactServiceServer.Grains.Contact;

public class ContactGrain : Grain, IContactGrain
{
    private readonly IContactAccess _contactAccess;
    private ContactState? _state = null;
    
    public ContactGrain(IContactAccess contactAccess)
    {
        _contactAccess = contactAccess;
    }

    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        _state = await _contactAccess.LoadState(this.GetPrimaryKey()); 
        
        await base.OnActivateAsync(cancellationToken);
    }

    public Task RegisterContact(string name)
    {
        throw new NotImplementedException();
    }

    public Task SetProfilePictureUrl(string url)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetName()
    {
        throw new NotImplementedException();
    }

    public Task<string> GetProfilePictureUrl()
    {
        throw new NotImplementedException();
    }

    public Task SetCompany(Organization company)
    {
        throw new NotImplementedException();
    }

    public Task<Organization> GetCompany()
    {
        throw new NotImplementedException();
    }

    public Task AddEmail(EmailAddress email)
    {
        throw new NotImplementedException();
    }

    public Task<EmailAddress[]> ListEmails()
    {
        throw new NotImplementedException();
    }

    public Task AddPhone(Phone phone)
    {
        throw new NotImplementedException();
    }

    public Task<Phone[]> ListPhones()
    {
        throw new NotImplementedException();
    }
}