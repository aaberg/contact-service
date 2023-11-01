using ContactServiceGrainInterfaces.Contact;
using ContactServiceGrainInterfaces.User;

namespace ContactServiceGrainInterfaces.Tenant;

public interface ITenantGrain : IGrainWithGuidKey
{
    Task RegisterTenant(TenantType type, string name);
    Task<string> GetName();

    Task<TenantType> GetTenantType();

    Task GiveUserAccess(IUserGrain user);
    Task<IUserGrain[]> ListUsers();
    
    Task<IContactGrain> CreateContact(string name);
    Task<IContactGrain[]> ListContacts();
}

