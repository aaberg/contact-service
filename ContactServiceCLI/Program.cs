using ContactServiceGrainInterfaces.Contact;
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

// create a couple of tenants
var privateTenantId = Guid.Parse("dde26d78-d64e-4933-92aa-0f69d882d40b");
var privateTenant = client.GetGrain<ITenantGrain>(privateTenantId);
await privateTenant.RegisterTenant(TenantType.Private, "Lars Aaberg");

var orgTenantId = Guid.Parse("8792444c-f42d-496f-abb8-1868cc801848");
var orgTenant = client.GetGrain<ITenantGrain>(orgTenantId);
await orgTenant.RegisterTenant(TenantType.Organization, "Senior Consultants AS");

// create a user
var user = client.GetGrain<IUserGrain>("user:1234");
await user.RegisterNewUser(new UserProfile("Lars Aaberg", "lars@aaberg.cc"));

// give the user access to the tenants
await privateTenant.GiveUserAccess(user);
await orgTenant.GiveUserAccess(user);

// create some contacts in the private tenant
var larsaaberg = await privateTenant.CreateContact("Lars Aaberg");
var erichalberd = await privateTenant.CreateContact("Eric Halberd");
var frankhudson = await privateTenant.CreateContact("Frank Hudson");

// create some contacts in the org tenant
var jonnytheman = await orgTenant.CreateContact("Jonny the man");
var sickrickinger = await orgTenant.CreateContact("Sick Rickinger");

// Update tenants with some more information
await larsaaberg.SetCompany(new Organization
{
    Company = "Senior Consultants AS",
    JobTitle = "Senior Consultant",
});

await larsaaberg.AddEmail(new EmailAddress("lars@aaberg.cc" ));
await larsaaberg.AddEmail(new EmailAddress("lars@aabergs.net"));

// List all tenants user have access to
var tenants = await user.GetTenantsWithAccess();
foreach (var tenantGrain in tenants)
{
    Console.WriteLine($"This user has access to {await tenantGrain.GetName()}");
}

// List all the contacts in the private tenant
var contacts = await privateTenant.ListContacts();

Console.WriteLine("Contacts for the private tenant");
foreach (var contactGrain in contacts)
{
    var emails = await contactGrain.ListEmails();
    
    Console.WriteLine($"Name: {await contactGrain.GetName()}, emails: {string.Join(", ", (await contactGrain.ListEmails()).Select(email => email.Email))}");
}