using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using B4.Data.DataFactory.Connections;
using B4.Data.DataFactory.Providers;
using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces;
using Dapper;

namespace B4.Data.DataFactory.Repositories
{
    /// <summary>
    /// Repositorio de DataActualsBw migrado a DataFactory.
    /// Resuelve la conexion y las queries por proveedor desde los componentes comunes.
    /// </summary>
    public sealed class DataActualsBwRepository : IDataActualsBwRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        /// <summary>
        /// Inicializa el repositorio con la factoria de conexiones y el proveedor de queries.
        /// </summary>
        /// <param name="connectionFactory">Factoría de conexiones a base de datos.</param>
        /// <param name="queryProvider">Origen de queries según proveedor.</param>
        public DataActualsBwRepository(
            IDbConnectionFactory connectionFactory,
            IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        /// <summary>
        /// Inserta un nuevo registro de DataActualsBw.
        /// </summary>
        /// <param name="entity">Entidad a insertar.</param>
        public async Task AddAsync(DataActualsBw entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            try
            {
                PrepareForWrite(entity, isInsert: true);

                using var conn = _connectionFactory.CreateConnection();

                if (IsPostgreSql())
                {
                    entity.Id = await conn.ExecuteScalarAsync<int>(
                        _queryProvider.DataActualsBwQueries.AddQuery(),
                        entity);
                }
                else
                {
                    await conn.ExecuteAsync(_queryProvider.DataActualsBwQueries.AddQuery(), entity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar un registro en DataActualsBw.", ex);
            }
        }

        /// <summary>
        /// Recupera un DataActualsBw por identificador.
        /// </summary>
        /// <param name="id">Identificador a buscar.</param>
        /// <returns>La entidad encontrada o null.</returns>
        public async Task<DataActualsBw?> GetByIdAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<DataActualsBw>(
                    _queryProvider.DataActualsBwQueries.GetByIdQuery(),
                    new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener DataActualsBw con id {id}.", ex);
            }
        }

        /// <summary>
        /// Recupera todos los registros de DataActualsBw.
        /// </summary>
        /// <returns>Colección de entidades.</returns>
        public async Task<IEnumerable<DataActualsBw>> GetAllAsync()
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                return await conn.QueryAsync<DataActualsBw>(
                    _queryProvider.DataActualsBwQueries.GetAllQuery());
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de DataActualsBw.", ex);
            }
        }

        /// <summary>
        /// Actualiza un registro existente de DataActualsBw.
        /// </summary>
        /// <param name="entity">Entidad a actualizar.</param>
        public async Task UpdateAsync(DataActualsBw entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            try
            {
                PrepareForWrite(entity, isInsert: false);

                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.DataActualsBwQueries.UpdateQuery(), entity);

                if (rows == 0)
                {
                    throw new Exception($"No se encontro el registro con Id {entity.Id} para actualizar.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el registro de DataActualsBw con Id {entity.Id}.", ex);
            }
        }

        /// <summary>
        /// Elimina un registro existente de DataActualsBw.
        /// </summary>
        /// <param name="id">Identificador a eliminar.</param>
        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(
                    _queryProvider.DataActualsBwQueries.DeleteQuery(),
                    new { Id = id });

                if (rows == 0)
                {
                    throw new Exception($"No se encontro el registro con Id {id} para eliminar.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el registro de DataActualsBw con Id {id}.", ex);
            }
        }

        private bool IsPostgreSql()
        {
            using var conn = _connectionFactory.CreateConnection();
            return conn.GetType().Name.Contains("Npgsql", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Prepara los campos tecnicos y financieros antes de escribir la entidad,
        /// respetando la diferencia actual entre MySQL y PostgreSQL.
        /// </summary>
        private void PrepareForWrite(DataActualsBw entity, bool isInsert)
        {
            if (IsPostgreSql())
            {
                var now = DateTime.UtcNow;

                if (isInsert)
                {
                    entity.CreatedAt = now;
                }

                entity.UpdatedAt = now;
                entity.RecalculateFinancialFields(increaseVersion: true);
                return;
            }

            entity.RecalculateFinancialFields(increaseVersion: false);
        }
    }
}
