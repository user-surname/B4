using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbUp;
using DbUp.Engine;
using DbUp.Helpers;
using DbUp.Support;
using DbUp.MySql;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Reflection;
using System;
using Microsoft.Extensions.Configuration;

namespace B4.Api.Extensions
{
    // Clase estática para gestionar las migraciones de base de datos
    public static class DbUpMigrator
    {

        // Método que asegura que la base de datos está actualizada a la última versión
        public static void EnsureDatabaseUpdated(IConfiguration configuration)
        {
            var bbdd = configuration["bbdd"]?.Trim();
            // Obtiene la cadena de conexión desde appsettings.json

            if (bbdd.Equals("MySQL", StringComparison.OrdinalIgnoreCase))
            {
                var connectionString = configuration.GetConnectionString("MySQLConnectionB4Data");
                // Configura el "upgrader" que aplicará las migraciones
                var upgrader =
                    DeployChanges.To.MySqlDatabase(connectionString) // Define el tipo de DB (MySQL)
                        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                        // Busca scripts SQL embebidos en el ensamblado actual (archivos .sql marcados como "Embedded Resource")
                        .LogToConsole() // Muestra el progreso y errores en la consola
                        .Build(); // Construye la instancia final de DbUp

                // Ejecuta las migraciones
                var result = upgrader.PerformUpgrade();

                // Si ocurre algún error durante las migraciones
                if (!result.Successful)
                {
                    Console.ForegroundColor = ConsoleColor.Red; // Texto rojo para resaltar error
                    Console.WriteLine(result.Error); // Muestra el error detallado
                    Console.ResetColor();
                    throw new Exception("Error applying migrations", result.Error); // Lanza excepción
                }

                // Si todo salió bien, mensaje de éxito en verde
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Database updated successfully!");
                Console.ResetColor();

            }
            else if (bbdd.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase))
            {
                var connectionString = configuration.GetConnectionString("PostgresConnectionB4Data");

                // Configura el "upgrader" que aplicará las migraciones
                var upgrader =
                    DeployChanges.To.PostgresqlDatabase(connectionString) // Define el tipo de DB (PostgreSQL)
                        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                        // Busca scripts SQL embebidos en el ensamblado actual (archivos .sql marcados como "Embedded Resource")
                        .LogToConsole() // Muestra el progreso y errores en la consola
                        .Build(); // Construye la instancia final de DbUp

                // Ejecuta las migraciones
                var result = upgrader.PerformUpgrade();

                // Si ocurre algún error durante las migraciones
                if (!result.Successful)
                {
                    Console.ForegroundColor = ConsoleColor.Red; // Texto rojo para resaltar error
                    Console.WriteLine(result.Error); // Muestra el error detallado
                    Console.ResetColor();
                    throw new Exception("Error applying migrations", result.Error); // Lanza excepción
                }

                // Si todo salió bien, mensaje de éxito en verde
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Database updated successfully!");
                Console.ResetColor();
            }
        }
    }
}
