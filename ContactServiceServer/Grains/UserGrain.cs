using ContactServiceGrainInterfaces.Tenant;
using ContactServiceGrainInterfaces.User;
using ContactServiceServer.DataAccess;
using ContactServiceServer.Grains.State;
using ContactServiceServer.Infrastructure;
using Orleans.Providers;

namespace ContactServiceServer.Grains;

[StorageProvider(ProviderName = Stores.Default)]
public class UserGrain : Grain<UserState>, IUserGrain
{
    private readonly ITenantAccess _tenantAccess;

    public UserGrain(ITenantAccess tenantAccess)
    {
        _tenantAccess = tenantAccess;
    }

    public Task RegisterNewUser(UserProfile userProfile)
    {
        State = new UserState
        {
            Profile = userProfile
        };
        
        return WriteStateAsync();
    }

    public Task<UserProfile> GetUserProfile()
    {
        return Task.FromResult(State.Profile);
    }

    public async Task<ITenantGrain[]> GetTenantsWithAccess()
    {
        var relations = await _tenantAccess.GetUserRelations(this.GetPrimaryKeyString());
        return relations.Select(relation => GrainFactory.GetGrain<ITenantGrain>(relation.TenantId)).ToArray();
    }

    public Task SelectTenant(ITenantGrain tenant)
    {
        State = State with { SelectedTenantId = tenant.GetPrimaryKey() };
        return WriteStateAsync();
    }

    public async Task<ITenantGrain?> GetSelectedTenant()
    {
        if (State.SelectedTenantId is null)
        {
            var tenants = await GetTenantsWithAccess();

            foreach (var t in tenants)
            {
                if (await t.GetTenantType() == TenantType.Private)
                {
                    return t;
                }
            }
        }

        return GrainFactory.GetGrain<ITenantGrain>(State.SelectedTenantId.Value)!; 
    }
}