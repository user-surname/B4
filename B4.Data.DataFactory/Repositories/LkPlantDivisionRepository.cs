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
    public sealed class LkPlantDivisionRepository : IPlantDivisionRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        public LkPlantDivisionRepository(IDbConnectionFactory connectionFactory, IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        public async Task AddAsync(LkPlantDivision entity)
        {
            try { using var conn = _connectionFactory.CreateConnection(); await conn.ExecuteAsync(_queryProvider.LkPlantDivisionQueries.AddQuery(), entity); }
            catch (Exception ex) { throw new Exception("Error al insertar PlantDivision.", ex); }
        }

        public async Task<LkPlantDivision?> GetByIdAsync(int id)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QuerySingleOrDefaultAsync<LkPlantDivision>(_queryProvider.LkPlantDivisionQueries.GetByIdQuery(), new { Id = id }); }
            catch (Exception ex) { throw new Exception($"Error al obtener PlantDivision con id {id}.", ex); }
        }

        public async Task<IEnumerable<LkPlantDivision>> GetAllAsync()
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<LkPlantDivision>(_queryProvider.LkPlantDivisionQueries.GetAllQuery()); }
            catch (Exception ex) { throw new Exception("Error al obtener la lista de PlantDivision.", ex); }
        }

        public async Task UpdateAsync(LkPlantDivision entity)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.LkPlantDivisionQueries.UpdateQuery(), entity);
                if (rows == 0) throw new Exception($"No se encontro PlantDivision para actualizar.");
            }
            catch (Exception ex) { throw new Exception("Error al actualizar PlantDivision.", ex); }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.LkPlantDivisionQueries.DeleteQuery(), new { Id = id });
                if (rows == 0) throw new Exception($"No se encontro PlantDivision con id {id} para eliminar.");
            }
            catch (Exception ex) { throw new Exception($"Error al eliminar PlantDivision con id {id}.", ex); }
        }
    }
}
