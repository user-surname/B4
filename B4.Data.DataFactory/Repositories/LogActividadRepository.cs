using B4.Data.DataFactory.Connections;
using B4.Data.DataFactory.Providers;
using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces;
using Dapper;

namespace B4.Data.DataFactory.Repositories
{
    public sealed class LogActividadRepository : ILogActividadRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        public LogActividadRepository(IDbConnectionFactory connectionFactory, IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        public async Task AddAsync(LogActividad entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                await conn.ExecuteAsync(_queryProvider.LogActividadQueries.AddQuery(), entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar log_actividad.", ex);
            }
        }

        public async Task<LogActividad?> GetByIdAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<LogActividad>(_queryProvider.LogActividadQueries.GetByIdQuery(), new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener log_actividad con id {id}.", ex);
            }
        }

        public async Task<IEnumerable<LogActividad>> GetAllAsync()
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                return await conn.QueryAsync<LogActividad>(_queryProvider.LogActividadQueries.GetAllQuery());
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de log_actividad.", ex);
            }
        }

        public async Task UpdateAsync(LogActividad entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.LogActividadQueries.UpdateQuery(), entity);
                if (rows == 0)
                {
                    throw new Exception($"No se encontro log_actividad con id {entity.Id} para actualizar.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar log_actividad con id {entity.Id}.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.LogActividadQueries.DeleteQuery(), new { Id = id });
                if (rows == 0)
                {
                    throw new Exception($"No se encontro log_actividad con id {id} para eliminar.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar log_actividad con id {id}.", ex);
            }
        }
    }
}
