using ContactServiceServer.DataAccess;
using ContactServiceServer.DataAccess.Contact;
using ContactServiceServer.DataAccess.Models;
using ContactServiceServer.Db;
using ContactServiceServer.Infrastructure;
using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Weasel.Core;

// configuration
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{environment}.json")
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

// logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Information()
    .CreateLogger();

var orleansConfiguration = new OrleansConfiguration();
configuration.GetRequiredSection("Orleans").Bind(orleansConfiguration);

new OrleansMigrations(orleansConfiguration, Log.Logger).Migrate();

var hostBuilder = Host.CreateDefaultBuilder(args);

hostBuilder
    .ConfigureHostConfiguration(builder =>
    {
        builder.Sources.Clear();
        builder.AddConfiguration(configuration);
    })

    .UseOrleans(silo =>
    {
        silo
            .UseLocalhostClustering()
            .ConfigureLogging(logging => logging.AddConsole())
            .AddAdoNetGrainStorage(Stores.Default, options =>
            {
                options.Invariant = "Npgsql";
                options.ConnectionString = orleansConfiguration.ConnectionString;
            })
            .ConfigureServices(services =>
            {
                services
                    .AddSingleton<ITenantAccess, TenantAccess>()
                    .AddMarten(options =>
                    {
                        options
                            .RegisterTenantAccessSchema()
                            .RegisterContactSchema()
                            .Connection(orleansConfiguration.ConnectionString);

                        if (environment == "Development")
                        {
                            options.AutoCreateSchemaObjects = AutoCreate.All;
                        }
                    });
            });
    })
    .UseConsoleLifetime();
    
using IHost host = hostBuilder.Build();


await host.RunAsync();