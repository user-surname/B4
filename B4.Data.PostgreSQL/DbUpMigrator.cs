using System;
using System.Reflection;
using DbUp;
using Microsoft.Extensions.Configuration;

namespace B4.Data.PostgreSQL
{
    // Clase estática para gestionar las migraciones de PostgreSQL
    public static class DbUpMigrator
    {
        public static void EnsureDatabaseUpdated(IConfiguration configuration)
        {
            // 👇 Igual que MySQL, pero con la key de Postgres
            var connectionString = configuration.GetConnectionString("PostgresConnectionB4Data");

            // ✅ Añadido: error claro si está vacía (evita el Regex.Match(null))
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Falta ConnectionStrings:PostgresConnectionB4Data en appsettings.json");

            var upgrader =
                DeployChanges.To
                    .PostgresqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
                throw new Exception("Error applying migrations (PostgreSQL)", result.Error);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("PostgreSQL database updated successfully!");
            Console.ResetColor();
        }
    }
}