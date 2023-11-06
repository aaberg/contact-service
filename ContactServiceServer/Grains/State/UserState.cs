using ContactServiceGrainInterfaces;
using ContactServiceGrainInterfaces.User;

namespace ContactServiceServer.Grains.State;

public record UserState
{
    public UserProfile? Profile { get; init; }

    public Guid? SelectedTenantId { get; init; }
}