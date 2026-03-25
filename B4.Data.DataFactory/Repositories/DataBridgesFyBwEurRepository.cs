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
    public sealed class DataBridgesFyBwEurRepository : IDataBridgesFyBwEurRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        public DataBridgesFyBwEurRepository(IDbConnectionFactory connectionFactory, IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        public async Task AddAsync(DataBridgesFyBwEur entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                await conn.ExecuteAsync(_queryProvider.DataBridgesFyBwEurQueries.AddQuery(), entity);
            }
            catch (Exception ex) { throw new Exception("Error al insertar DataBridgesFyBwEur.", ex); }
        }

        public async Task<DataBridgesFyBwEur?> GetByIdAsync(int id)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QuerySingleOrDefaultAsync<DataBridgesFyBwEur>(_queryProvider.DataBridgesFyBwEurQueries.GetByIdQuery(), new { Id = id }); }
            catch (Exception ex) { throw new Exception($"Error al obtener DataBridgesFyBwEur con id {id}.", ex); }
        }

        public async Task<IEnumerable<DataBridgesFyBwEur>> GetAllAsync()
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<DataBridgesFyBwEur>(_queryProvider.DataBridgesFyBwEurQueries.GetAllQuery()); }
            catch (Exception ex) { throw new Exception("Error al obtener la lista de DataBridgesFyBwEur.", ex); }
        }

        public async Task UpdateAsync(DataBridgesFyBwEur entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.DataBridgesFyBwEurQueries.UpdateQuery(), entity);
                if (rows == 0) throw new Exception($"No se encontro DataBridgesFyBwEur con id {entity.Id} para actualizar.");
            }
            catch (Exception ex) { throw new Exception($"Error al actualizar DataBridgesFyBwEur con id {entity.Id}.", ex); }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.DataBridgesFyBwEurQueries.DeleteQuery(), new { Id = id });
                if (rows == 0) throw new Exception($"No se encontro DataBridgesFyBwEur con id {id} para eliminar.");
            }
            catch (Exception ex) { throw new Exception($"Error al eliminar DataBridgesFyBwEur con id {id}.", ex); }
        }

        public async Task<IEnumerable<DataBridgesFyBwEur>> GetByPlantaEjercicioAsync(int planta, int ejercicio)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<DataBridgesFyBwEur>(_queryProvider.DataBridgesFyBwEurQueries.GetByPlantaEjercicioQuery(), new { planta, ejercicio }); }
            catch (Exception ex) { throw new Exception($"Error al obtener DataBridgesFyBwEur para planta {planta} ejercicio {ejercicio}.", ex); }
        }
    }
}
