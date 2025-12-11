using DbUp;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using DbUp.Postgresql;
using System.Threading;



namespace B4.Data.PostgreSQL
{
    public static class DbUpMigrator
    {
        public static void EnsureDatabaseUpdated(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PostgresConnection");

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
                throw new Exception("Error applying migrations", result.Error);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("PostgreSQL database updated successfully!");
            Console.ResetColor();
        }
    }
}
