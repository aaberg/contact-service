using ContactServiceGrainInterfaces;
using ContactServiceGrainInterfaces.Tenant;
using ContactServiceGrainInterfaces.User;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args)
    .UseOrleansClient(client =>
    {
        client.UseLocalhostClustering();
    })
    
    .ConfigureLogging(logging => logging.AddConsole())
    .UseConsoleLifetime();
    
IHost host = hostBuilder.Build();
await host.StartAsync();


// stuff
var client = host.Services.GetRequiredService<IClusterClient>();
var log = host.Services.GetRequiredService<ILogger<Program>>();

var privateTenantId = Guid.Parse("dde26d78-d64e-4933-92aa-0f69d882d40b");
var privateTenant = client.GetGrain<ITenantGrain>(privateTenantId);
await privateTenant.RegisterTenant(TenantType.Private, "Lars Aaberg");

var orgTenantId = Guid.NewGuid();
var orgTenant = client.GetGrain<ITenantGrain>(orgTenantId);
await orgTenant.RegisterTenant(TenantType.Organization, "Sonat AS");

var user = client.GetGrain<IUserGrain>("user:1234");
await user.RegisterNewUser(new UserProfile("Lars Aaberg", "lars@aaberg.cc"));

await privateTenant.GiveUserAccess(user);
await orgTenant.GiveUserAccess(user);


var tenants = await user.GetTenantsWithAccess();
foreach (var tenantGrain in tenants)
{
    Console.WriteLine($"This user has access to {await tenantGrain.GetName()}");
}