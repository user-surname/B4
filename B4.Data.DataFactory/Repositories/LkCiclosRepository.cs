using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using B4.Data.DataFactory.Connections;
using B4.Data.DataFactory.Providers;
using B4.Models.Entities.LkEntities;
using B4.Models.RepositoryInterfaces.LkInterfaces;
using Dapper;

namespace B4.Data.DataFactory.Repositories
{
    public sealed class LkCiclosRepository : ICiclosRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        public LkCiclosRepository(IDbConnectionFactory connectionFactory, IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        public async Task AddAsync(LkCiclos entity)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                await conn.ExecuteAsync(_queryProvider.LkCiclosQueries.AddQuery(), entity);
            }
            catch (Exception ex) { throw new Exception("Error al insertar un Ciclo.", ex); }
        }

        public async Task<LkCiclos?> GetByIdAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<LkCiclos>(_queryProvider.LkCiclosQueries.GetByIdQuery(), new { Id = id });
            }
            catch (Exception ex) { throw new Exception($"Error al obtener el Ciclo con id {id}.", ex); }
        }

        public async Task<IEnumerable<LkCiclos>> GetAllAsync()
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                return await conn.QueryAsync<LkCiclos>(_queryProvider.LkCiclosQueries.GetAllQuery());
            }
            catch (Exception ex) { throw new Exception("Error al obtener la lista de Ciclos.", ex); }
        }

        public async Task UpdateAsync(LkCiclos entity)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.LkCiclosQueries.UpdateQuery(), entity);
                if (rows == 0) throw new Exception($"No se encontro el Ciclo con id {entity.Id} para actualizar.");
            }
            catch (Exception ex) { throw new Exception($"Error al actualizar el Ciclo con id {entity.Id}.", ex); }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.LkCiclosQueries.DeleteQuery(), new { Id = id });
                if (rows == 0) throw new Exception($"No se encontro el Ciclo con id {id} para eliminar.");
            }
            catch (Exception ex) { throw new Exception($"Error al eliminar el Ciclo con id {id}.", ex); }
        }
    }
}
