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
    public sealed class DataBridgesFyBwRepository : IDataBridgesFyBwRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        public DataBridgesFyBwRepository(IDbConnectionFactory connectionFactory, IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        public async Task AddAsync(DataBridgesFyBw entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                await conn.ExecuteAsync(_queryProvider.DataBridgesFyBwQueries.AddQuery(), entity);
            }
            catch (Exception ex) { throw new Exception("Error al insertar DataBridgesFyBw.", ex); }
        }

        public async Task<DataBridgesFyBw?> GetByIdAsync(int id)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QuerySingleOrDefaultAsync<DataBridgesFyBw>(_queryProvider.DataBridgesFyBwQueries.GetByIdQuery(), new { Id = id }); }
            catch (Exception ex) { throw new Exception($"Error al obtener DataBridgesFyBw con id {id}.", ex); }
        }

        public async Task<IEnumerable<DataBridgesFyBw>> GetAllAsync()
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<DataBridgesFyBw>(_queryProvider.DataBridgesFyBwQueries.GetAllQuery()); }
            catch (Exception ex) { throw new Exception("Error al obtener la lista de DataBridgesFyBw.", ex); }
        }

        public async Task UpdateAsync(DataBridgesFyBw entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.DataBridgesFyBwQueries.UpdateQuery(), entity);
                if (rows == 0) throw new Exception($"No se encontro DataBridgesFyBw con id {entity.Id} para actualizar.");
            }
            catch (Exception ex) { throw new Exception($"Error al actualizar DataBridgesFyBw con id {entity.Id}.", ex); }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.DataBridgesFyBwQueries.DeleteQuery(), new { Id = id });
                if (rows == 0) throw new Exception($"No se encontro DataBridgesFyBw con id {id} para eliminar.");
            }
            catch (Exception ex) { throw new Exception($"Error al eliminar DataBridgesFyBw con id {id}.", ex); }
        }

        public async Task<IEnumerable<DataBridgesFyBw>> GetByPlantaEjercicioAsync(int planta, int ejercicio)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<DataBridgesFyBw>(_queryProvider.DataBridgesFyBwQueries.GetByPlantaEjercicioQuery(), new { planta, ejercicio }); }
            catch (Exception ex) { throw new Exception($"Error al obtener DataBridgesFyBw para planta {planta} ejercicio {ejercicio}.", ex); }
        }
    }
}
