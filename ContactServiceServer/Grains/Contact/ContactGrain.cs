using ContactServiceGrainInterfaces.Contact;
using ContactServiceGrainInterfaces.Tenant;
using ContactServiceServer.DataAccess.Contact;
using ContactServiceServer.Exceptions;

namespace ContactServiceServer.Grains.Contact;

public class ContactGrain(IContactAccess contactAccess) : Grain, IContactGrain
{
    private ContactState? _state;

    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        _state = await contactAccess.LoadStateAsync(this.GetPrimaryKey()); 
        
        await base.OnActivateAsync(cancellationToken);
    }

    public async Task RegisterContact(ITenantGrain ownerTenant, string name)
    {
        if (_state != null)
        {
            throw new DomainException("Contact already registered");
        }

        _state = new ContactState
        {
            OwnerTenantId = ownerTenant.GetPrimaryKey(),
            Name = name
        };
        await SaveStateAsync();
    }

    public async Task SetProfilePictureUrl(string url)
    {
        ThrowIfNotRegistered();
        _state = _state! with
        {
            ProfilePictureUrl = url
        };
        await SaveStateAsync();
    }

    public Task<string> GetName()
    {
        ThrowIfNotRegistered();
        return Task.FromResult(_state!.Name);
    }

    public Task<string?> GetProfilePictureUrl()
    {
        ThrowIfNotRegistered();
        return Task.FromResult(_state!.ProfilePictureUrl);
    }

    public async Task SetCompany(Organization company)
    {
        ThrowIfNotRegistered();
        _state = _state! with
        {
            Company = company
        };
        await SaveStateAsync();
    }

    public Task<Organization> GetCompany()
    {
        ThrowIfNotRegistered();
        return Task.FromResult(_state!.Company!);
    }

    public async Task AddEmail(EmailAddress email)
    {
        ThrowIfNotRegistered();
        _state = _state! with
        {
            Emails = _state!.Emails.Append(email).ToArray()
        };
        await SaveStateAsync();
    }

    public Task<EmailAddress[]> ListEmails()
    {
        ThrowIfNotRegistered();
        return Task.FromResult(_state!.Emails);
    }

    public async Task AddPhone(Phone phone)
    {
        ThrowIfNotRegistered();
        _state = _state! with
        {
            PhoneNumbers = _state!.PhoneNumbers.Append(phone).ToArray()
        };
        await SaveStateAsync();
    }

    public Task<Phone[]> ListPhones()
    {
        ThrowIfNotRegistered();
        return Task.FromResult(_state!.PhoneNumbers);
    }
    
    private void ThrowIfNotRegistered()
    {
        if (_state == null)
        {
            throw new DomainException("Error accessing contact state because it is null. Check that the contact is registered");
        }
    }
    
    private async Task SaveStateAsync()
    {
        if (_state == null)
        {
            throw new DomainException("Cant save Contact without state");
        }
        await contactAccess.SaveStateAsync(this.GetPrimaryKey(), _state);
    }
}