using ContactServiceGrainInterfaces;
using ContactServiceGrainInterfaces.Contact;
using ContactServiceGrainInterfaces.Tenant;
using ContactServiceGrainInterfaces.User;
using ContactServiceServer.DataAccess;
using ContactServiceServer.DataAccess.Contact;
using ContactServiceServer.Grains.State;
using ContactServiceServer.Infrastructure;
using Orleans.Providers;

namespace ContactServiceServer.Grains;

[StorageProvider(ProviderName = Stores.Default)]
public class TenantGrain : Grain<TenantState>, ITenantGrain
{
    private readonly ITenantAccess _tenantAccess;
    private readonly IContactAccess _contactAccess;

    public TenantGrain(ITenantAccess tenantAccess, IContactAccess contactAccess)
    {
        _tenantAccess = tenantAccess;
        _contactAccess = contactAccess;
    }

    public Task RegisterTenant(TenantType type, string name)
    {
        State = new TenantState { Type = type, Name = name };
        return WriteStateAsync();
    }

    public Task<string> GetName()
    {
        return Task.FromResult(State.Name);
    }

    public Task<TenantType> GetTenantType()
    {
        return Task.FromResult(State.Type);
    }

    public async Task GiveUserAccess(IUserGrain user)
    {
        await _tenantAccess.GiveUserAccessToTenant(this.GetPrimaryKey(), user.GetPrimaryKeyString());
    }

    public async Task<IUserGrain[]> ListUsers()
    {
        var relations = await _tenantAccess.GetTenantRelations(this.GetPrimaryKey());
        return relations
            .Select(relationEntry => GrainFactory.GetGrain<IUserGrain>(relationEntry.UserId))
            .ToArray();
    }

    public async Task<IContactGrain> CreateContact(string name)
    {
        var contactId = Guid.NewGuid();
        var contactGrain = GrainFactory.GetGrain<IContactGrain>(contactId);
        await contactGrain.RegisterContact(this, name);
        return contactGrain;
    }

    public async Task<IContactGrain[]> ListContacts()
    {
        var relations = await _contactAccess.ListTenantContactsAsync(this.GetPrimaryKey());
        return relations.Select(relation => GrainFactory.GetGrain<IContactGrain>(relation.ContactId)).ToArray();
    }
}