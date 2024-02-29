using ContactServiceServer.Infrastructure;
using EvolveDb;
using Npgsql;
using Serilog;

namespace ContactServiceServer.Db;

public class OrleansMigrations
{
    private readonly DatabaseConfiguration _configuration;
    private readonly ILogger _log;

    public OrleansMigrations(DatabaseConfiguration configuration, ILogger logger)
    {
        _configuration = configuration;
        _log = logger;
    }

    public void Migrate()
    {
        //using var connection = new NpgsqlConnection("Server=127.0.0.1;Port=15432;Database=postgres;User Id=postgres;Password=secret;");
        using var connection = new NpgsqlConnection(_configuration.ConnectionString);

        var evolve = new Evolve(connection, _log.Information)
        {
            Locations = new[] { "Db/orleans/migrations" },
            IsEraseDisabled = true,
        };
        
        evolve.Migrate();
    }
}