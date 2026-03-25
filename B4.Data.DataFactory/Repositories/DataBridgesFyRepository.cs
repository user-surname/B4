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
    public sealed class DataBridgesFyRepository : IDataBridgesFyRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        public DataBridgesFyRepository(IDbConnectionFactory connectionFactory, IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        public async Task AddAsync(DataBridgesFy entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                await conn.ExecuteAsync(_queryProvider.DataBridgesFyQueries.AddQuery(), entity);
            }
            catch (Exception ex) { throw new Exception("Error al insertar DataBridgesFy.", ex); }
        }

        public async Task<DataBridgesFy?> GetByIdAsync(int id)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QuerySingleOrDefaultAsync<DataBridgesFy>(_queryProvider.DataBridgesFyQueries.GetByIdQuery(), new { Id = id }); }
            catch (Exception ex) { throw new Exception($"Error al obtener DataBridgesFy con id {id}.", ex); }
        }

        public async Task<IEnumerable<DataBridgesFy>> GetAllAsync()
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<DataBridgesFy>(_queryProvider.DataBridgesFyQueries.GetAllQuery()); }
            catch (Exception ex) { throw new Exception("Error al obtener la lista de DataBridgesFy.", ex); }
        }

        public async Task UpdateAsync(DataBridgesFy entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.DataBridgesFyQueries.UpdateQuery(), entity);
                if (rows == 0) throw new Exception($"No se encontro DataBridgesFy con id {entity.Id} para actualizar.");
            }
            catch (Exception ex) { throw new Exception($"Error al actualizar DataBridgesFy con id {entity.Id}.", ex); }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.DataBridgesFyQueries.DeleteQuery(), new { Id = id });
                if (rows == 0) throw new Exception($"No se encontro DataBridgesFy con id {id} para eliminar.");
            }
            catch (Exception ex) { throw new Exception($"Error al eliminar DataBridgesFy con id {id}.", ex); }
        }

        public async Task<IEnumerable<DataBridgesFy>> GetByPlantaEjercicioAsync(int planta, int ejercicio)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<DataBridgesFy>(_queryProvider.DataBridgesFyQueries.GetByPlantaEjercicioQuery(), new { planta, ejercicio }); }
            catch (Exception ex) { throw new Exception($"Error al obtener DataBridgesFy para planta {planta} ejercicio {ejercicio}.", ex); }
        }
    }
}
