using System.Data;

namespace B4.Data.DataFactory.Connections
{
    /// <summary>
    /// Define la factoria comun que usa DataFactory para abrir la conexion
    /// correcta segun el proveedor activo configurado en la aplicacion.
    /// </summary>
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// Crea una nueva conexion a base de datos para MySQL o PostgreSQL
        /// segun el valor configurado en la clave "bbdd".
        /// </summary>
        /// <returns>Conexion lista para usar.</returns>
        IDbConnection CreateConnection();
    }
}
