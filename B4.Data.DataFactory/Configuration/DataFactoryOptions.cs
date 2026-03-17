namespace B4.Data.DataFactory.Configuration
{
    /// <summary>
    /// Almacena el proveedor activo y las cadenas de conexión usadas por DataFactory.
    /// </summary>
    public sealed class DataFactoryOptions
    {
        /// <summary>
        /// Clave de configuración usada para seleccionar el proveedor activo.
        /// </summary>
        public const string ProviderConfigurationKey = "bbdd";

        /// <summary>
        /// Nombre de la cadena de conexión para la base de datos de datos en MySQL.
        /// </summary>
        public const string MySqlConnectionStringName = "MySQLConnectionB4Data";

        /// <summary>
        /// Nombre de la cadena de conexión para la base de datos de datos en PostgreSQL.
        /// </summary>
        public const string PostgreSqlConnectionStringName = "PostgresConnectionB4Data";

        /// <summary>
        /// Nombre reservado para una futura cadena de conexión de SQL Server.
        /// </summary>
        public const string SqlServerConnectionStringName = "SqlServerConnectionB4Data";

        /// <summary>
        /// Obtiene o establece el proveedor activo leído desde configuración.
        /// </summary>
        public string Provider { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la cadena de conexión de MySQL.
        /// </summary>
        public string MySqlConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la cadena de conexión de PostgreSQL.
        /// </summary>
        public string PostgreSqlConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la cadena de conexión de SQL Server para uso futuro.
        /// </summary>
        public string SqlServerConnectionString { get; set; } = string.Empty;
    }
}
