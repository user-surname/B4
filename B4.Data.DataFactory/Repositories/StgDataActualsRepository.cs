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
    /// Repositorio de DataActuals migrado a DataFactory.
    /// Resuelve la conexion y las queries por proveedor sin depender de los repositorios antiguos.
    /// </summary>
    public sealed class StgDataActualsRepository : IStgDataActualsRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        /// <summary>
        /// Inicializa el repositorio con la factoria de conexiones y el proveedor de queries.
        /// </summary>
        /// <param name="connectionFactory">Factoria de conexiones a base de datos.</param>
        /// <param name="queryProvider">Origen de queries segun proveedor.</param>
        public StgDataActualsRepository(
            IDbConnectionFactory connectionFactory,
            IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        /// <summary>
        /// Inserta un nuevo registro de DataActuals.
        /// </summary>
        /// <param name="entity">Entidad a insertar.</param>
        public async Task AddAsync(StgDataActuals entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            try
            {
                PrepareForWrite(entity, isInsert: true);

                using var conn = _connectionFactory.CreateConnection();

                if (IsPostgreSql())
                {
                    entity.Id = await conn.ExecuteScalarAsync<int>(
                        _queryProvider.
                        StgDataActualsQueries.AddQuery(),
                        entity);
                }
                else
                {
                    await conn.ExecuteAsync(_queryProvider.StgDataActualsQueries.AddQuery(), entity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar un registro en DataActuals.", ex);
            }
        }

        /// <summary>
        /// Recupera un DataActuals por identificador.
        /// </summary>
        /// <param name="id">Identificador a buscar.</param>
        /// <returns>La entidad encontrada o null.</returns>
        public async Task<StgDataActuals?> GetByIdAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<StgDataActuals>(
                    _queryProvider.StgDataActualsQueries.GetByIdQuery(),
                    new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener DataActuals con id {id}.", ex);
            }
        }

        /// <summary>
        /// Recupera todos los registros de DataActuals.
        /// </summary>
        /// <returns>Coleccion de entidades.</returns>
        public async Task<IEnumerable<StgDataActuals>> GetAllAsync()
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                return await conn.QueryAsync<StgDataActuals>(
                    _queryProvider.StgDataActualsQueries.GetAllQuery());
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de DataActuals.", ex);
            }
        }

        /// <summary>
        /// Actualiza un registro existente de DataActuals.
        /// </summary>
        /// <param name="entity">Entidad a actualizar.</param>
        public async Task UpdateAsync(StgDataActuals entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            try
            {
                PrepareForWrite(entity, isInsert: false);

                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.StgDataActualsQueries.UpdateQuery(), entity);

                if (rows == 0)
                {
                    throw new Exception($"No se encontro el registro con Id {entity.Id} para actualizar.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el registro de DataActuals con Id {entity.Id}.", ex);
            }
        }

        /// <summary>
        /// Elimina un registro existente de DataActuals.
        /// </summary>
        /// <param name="id">Identificador a eliminar.</param>
        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(
                    _queryProvider.StgDataActualsQueries.DeleteQuery(),
                    new { Id = id });

                if (rows == 0)
                {
                    throw new Exception($"No se encontro el registro con Id {id} para eliminar.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el registro de DataActuals con Id {id}.", ex);
            }
        }

        /// <summary>
        /// Recupera los registros de DataActuals de una planta y ejercicio concretos.
        /// </summary>
        /// <param name="planta">Identificador de la planta.</param>
        /// <param name="ejercicio">Ejercicio a consultar.</param>
        /// <returns>Coleccion de registros encontrados.</returns>
        public async Task<IEnumerable<StgDataActuals>> GetByPlantaEjercicioAsync(int planta, int ejercicio)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                return await conn.QueryAsync<StgDataActuals>(
                    _queryProvider.StgDataActualsQueries.GetByPlantaEjercicioQuery(),
                    new { planta, ejercicio });
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error al obtener DataActuals para la planta {planta} y ejercicio {ejercicio}.",
                    ex);
            }
        }

        /// <summary>
        /// Recupera un registro de DataActuals por planta, ejercicio y epigrafe.
        /// </summary>
        /// <param name="planta">Identificador de la planta.</param>
        /// <param name="ejercicio">Ejercicio a consultar.</param>
        /// <param name="epigrafe">Epigrafe a consultar.</param>
        /// <returns>La entidad encontrada o null.</returns>
        public async Task<StgDataActuals?> GetByPlantaEjercicioEpigrafeAsync(int planta, int ejercicio, int epigrafe)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<StgDataActuals>(
                    _queryProvider.StgDataActualsQueries.GetByPlantaEjercicioEpigrafeQuery(),
                    new { planta, ejercicio, epigrafe });
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error al obtener DataActuals para la planta {planta}, ejercicio {ejercicio} y epigrafe {epigrafe}.",
                    ex);
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
        private void PrepareForWrite(StgDataActuals entity, bool isInsert)
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
