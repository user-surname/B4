using Microsoft.Extensions.Configuration;

public static class TestConfig
{
    public const string Conn =
        "Host=85.215.152.131;Port=15433;Database=b4_data;Username=b4_user;Password=b4_adminsecret852AKD;Trust Server Certificate=true;";

    public static IConfiguration Configuration { get; }

    static TestConfig()
    {
        Configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                // ✅ esta es la que tu contexto está pidiendo ahora
                { "ConnectionStrings:PostgresConnectionB4Data", Conn },

                // (opcional) por si algún otro contexto usa otra
                { "ConnectionStrings:PostgresConnectionB4Control",
                  "Host=85.215.152.131;Port=15433;Database=b4_control;Username=b4_user;Password=b4_usersecret852AKD;" }
            })
            .Build();
    }
}