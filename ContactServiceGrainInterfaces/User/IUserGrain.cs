using ContactServiceGrainInterfaces.Tenant;

namespace ContactServiceGrainInterfaces.User;

public interface IUserGrain : IGrainWithStringKey
{
    Task RegisterNewUser(UserProfile userProfile);

    Task<UserProfile> GetUserProfile();

    Task<ITenantGrain[]> GetTenantsWithAccess();
}
