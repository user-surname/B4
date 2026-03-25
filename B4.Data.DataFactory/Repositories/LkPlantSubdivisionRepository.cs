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
    public sealed class LkPlantSubdivisionRepository : IPlantSubdivisionRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        public LkPlantSubdivisionRepository(IDbConnectionFactory connectionFactory, IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        public async Task AddAsync(LkPlantSubdivision entity)
        {
            try { using var conn = _connectionFactory.CreateConnection(); await conn.ExecuteAsync(_queryProvider.LkPlantSubdivisionQueries.AddQuery(), entity); }
            catch (Exception ex) { throw new Exception("Error al insertar PlantSubdivision.", ex); }
        }

        public async Task<LkPlantSubdivision?> GetByIdAsync(int id)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QuerySingleOrDefaultAsync<LkPlantSubdivision>(_queryProvider.LkPlantSubdivisionQueries.GetByIdQuery(), new { Id = id }); }
            catch (Exception ex) { throw new Exception($"Error al obtener PlantSubdivision con id {id}.", ex); }
        }

        public async Task<IEnumerable<LkPlantSubdivision>> GetAllAsync()
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<LkPlantSubdivision>(_queryProvider.LkPlantSubdivisionQueries.GetAllQuery()); }
            catch (Exception ex) { throw new Exception("Error al obtener la lista de PlantSubdivision.", ex); }
        }

        public async Task UpdateAsync(LkPlantSubdivision entity)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.LkPlantSubdivisionQueries.UpdateQuery(), entity);
                if (rows == 0) throw new Exception($"No se encontro PlantSubdivision para actualizar.");
            }
            catch (Exception ex) { throw new Exception("Error al actualizar PlantSubdivision.", ex); }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.LkPlantSubdivisionQueries.DeleteQuery(), new { Id = id });
                if (rows == 0) throw new Exception($"No se encontro PlantSubdivision con id {id} para eliminar.");
            }
            catch (Exception ex) { throw new Exception($"Error al eliminar PlantSubdivision con id {id}.", ex); }
        }
    }
}
