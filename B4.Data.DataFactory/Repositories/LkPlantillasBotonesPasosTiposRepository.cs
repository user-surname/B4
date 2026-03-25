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
    public sealed class LkPlantillasBotonesPasosTiposRepository : IPlantillasBotonesPasosTiposRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        public LkPlantillasBotonesPasosTiposRepository(IDbConnectionFactory connectionFactory, IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        public async Task AddAsync(LkPlantillasBotonesPasosTipos entity)
        {
            try { using var conn = _connectionFactory.CreateConnection(); await conn.ExecuteAsync(_queryProvider.LkPlantillasBotonesPasosTiposQueries.AddQuery(), entity); }
            catch (Exception ex) { throw new Exception("Error al insertar PlantillasBotonesPasosTipos.", ex); }
        }

        public async Task<LkPlantillasBotonesPasosTipos?> GetByIdAsync(int id)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QuerySingleOrDefaultAsync<LkPlantillasBotonesPasosTipos>(_queryProvider.LkPlantillasBotonesPasosTiposQueries.GetByIdQuery(), new { Id = id }); }
            catch (Exception ex) { throw new Exception($"Error al obtener PlantillasBotonesPasosTipos con id {id}.", ex); }
        }

        public async Task<IEnumerable<LkPlantillasBotonesPasosTipos>> GetAllAsync()
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<LkPlantillasBotonesPasosTipos>(_queryProvider.LkPlantillasBotonesPasosTiposQueries.GetAllQuery()); }
            catch (Exception ex) { throw new Exception("Error al obtener la lista de PlantillasBotonesPasosTipos.", ex); }
        }

        public async Task UpdateAsync(LkPlantillasBotonesPasosTipos entity)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.LkPlantillasBotonesPasosTiposQueries.UpdateQuery(), entity);
                if (rows == 0) throw new Exception($"No se encontro PlantillasBotonesPasosTipos para actualizar.");
            }
            catch (Exception ex) { throw new Exception("Error al actualizar PlantillasBotonesPasosTipos.", ex); }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.LkPlantillasBotonesPasosTiposQueries.DeleteQuery(), new { Id = id });
                if (rows == 0) throw new Exception($"No se encontro PlantillasBotonesPasosTipos con id {id} para eliminar.");
            }
            catch (Exception ex) { throw new Exception($"Error al eliminar PlantillasBotonesPasosTipos con id {id}.", ex); }
        }
    }
}
