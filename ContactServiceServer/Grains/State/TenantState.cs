using ContactServiceGrainInterfaces;
using ContactServiceGrainInterfaces.Tenant;

namespace ContactServiceServer.Grains.State;

public record TenantState
{
    public required string Name { get; init; }
    public required TenantType Type { get; init; }
}