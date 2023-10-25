using ContactServiceGrainInterfaces;
using ContactServiceGrainInterfaces.Tenant;
using ContactServiceGrainInterfaces.User;
using ContactServiceServer.DataAccess;
using ContactServiceServer.Grains.State;
using ContactServiceServer.Infrastructure;
using Orleans.Providers;

namespace ContactServiceServer.Grains;

[StorageProvider(ProviderName = Stores.Default)]
public class TenantGrain : Grain<TenantState>, ITenantGrain
{
    private readonly ITenantAccess _tenantAccess;

    public TenantGrain(ITenantAccess tenantAccess)
    {
        _tenantAccess = tenantAccess;
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
}