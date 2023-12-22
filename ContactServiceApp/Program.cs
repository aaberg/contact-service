using ContactServiceApp.Components;
using ContactServiceApp.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Configuration;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services
    .AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddApplicationCookie();

builder.Services
    .AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IUserStore<ApplicationUser>, UserStore>();

builder.Services
    .AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
    //.AddScoped<AuthenticationService>();

builder.Services
    .AddControllers();

builder.Host.UseOrleansClient(clientBuilder =>
{
    clientBuilder.UseLocalhostClustering();
});

    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseResponseCompression();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.Run();
