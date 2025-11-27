using Microsoft.Extensions.Configuration;

public static class TestConfig
{
    // Cadena de conexión única para TODOS los tests PostgreSQL
    public const string Conn =
        "Host=pg-3105f5b7-elpuig-23d9.b.aivencloud.com;Port=21134;Database=defaultdb;Username=avnadmin;Password=AVNS_D8EOFxPDlpUH6LhFFLw;Ssl Mode=Require;";

    // Configuración para DapperContext si lo necesitas
    public static IConfiguration Configuration { get; }

    static TestConfig()
    {
        var builder = new ConfigurationBuilder();

        builder.AddInMemoryCollection(new Dictionary<string, string?>
        {
            { "ConnectionStrings:PostgresAiven", Conn }
        });

        Configuration = builder.Build();
    }
}
