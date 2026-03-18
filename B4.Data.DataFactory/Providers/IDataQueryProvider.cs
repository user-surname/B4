using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Providers
{
    /// <summary>
    /// Expone los grupos de queries SQL ya resueltos para el proveedor activo.
    /// Los repositorios migrados consumen este contrato y no deciden por si mismos
    /// si trabajan contra MySQL o PostgreSQL.
    /// </summary>
    public interface IDataQueryProvider
    {
        /// <summary>
        /// Obtiene las queries de Usuario para el proveedor activo.
        /// </summary>
        IUsuarioQueries UsuarioQueries { get; }

        /// <summary>
        /// Obtiene las queries de ControlPlanta para el proveedor activo.
        /// </summary>
        IControlPlantaQueries ControlPlantaQueries { get; }

        /// <summary>
        /// Obtiene las queries de DataActualsBw para el proveedor activo.
        /// </summary>
        IDataActualsBwQueries DataActualsBwQueries { get; }

        /// <summary>
        /// Obtiene las queries de DataActuals para el proveedor activo.
        /// </summary>
        IDataActualsQueries DataActualsQueries { get; }
    }
}
