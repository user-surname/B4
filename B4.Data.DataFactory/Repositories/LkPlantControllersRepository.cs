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
    public sealed class LkPlantControllersRepository : IPlantControllersRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        public LkPlantControllersRepository(IDbConnectionFactory connectionFactory, IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        public async Task AddAsync(LkPlantControllers entity)
        {
            try { using var conn = _connectionFactory.CreateConnection(); await conn.ExecuteAsync(_queryProvider.LkPlantControllersQueries.AddQuery(), entity); }
            catch (Exception ex) { throw new Exception("Error al insertar PlantControllers.", ex); }
        }

        public async Task<LkPlantControllers?> GetByIdAsync(int id)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QuerySingleOrDefaultAsync<LkPlantControllers>(_queryProvider.LkPlantControllersQueries.GetByIdQuery(), new { Id = id }); }
            catch (Exception ex) { throw new Exception($"Error al obtener PlantControllers con id {id}.", ex); }
        }

        public async Task<IEnumerable<LkPlantControllers>> GetAllAsync()
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<LkPlantControllers>(_queryProvider.LkPlantControllersQueries.GetAllQuery()); }
            catch (Exception ex) { throw new Exception("Error al obtener la lista de PlantControllers.", ex); }
        }

        public async Task UpdateAsync(LkPlantControllers entity)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.LkPlantControllersQueries.UpdateQuery(), entity);
                if (rows == 0) throw new Exception($"No se encontro PlantControllers para actualizar.");
            }
            catch (Exception ex) { throw new Exception("Error al actualizar PlantControllers.", ex); }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.LkPlantControllersQueries.DeleteQuery(), new { Id = id });
                if (rows == 0) throw new Exception($"No se encontro PlantControllers con id {id} para eliminar.");
            }
            catch (Exception ex) { throw new Exception($"Error al eliminar PlantControllers con id {id}.", ex); }
        }
    }
}
