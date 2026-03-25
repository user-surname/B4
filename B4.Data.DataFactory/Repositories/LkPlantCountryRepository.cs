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
    public sealed class LkPlantCountryRepository : IPlantCountryRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        public LkPlantCountryRepository(IDbConnectionFactory connectionFactory, IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        public async Task AddAsync(LkPlantCountry entity)
        {
            try { using var conn = _connectionFactory.CreateConnection(); await conn.ExecuteAsync(_queryProvider.LkPlantCountryQueries.AddQuery(), entity); }
            catch (Exception ex) { throw new Exception("Error al insertar PlantCountry.", ex); }
        }

        public async Task<LkPlantCountry?> GetByIdAsync(int id)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QuerySingleOrDefaultAsync<LkPlantCountry>(_queryProvider.LkPlantCountryQueries.GetByIdQuery(), new { Id = id }); }
            catch (Exception ex) { throw new Exception($"Error al obtener PlantCountry con id {id}.", ex); }
        }

        public async Task<IEnumerable<LkPlantCountry>> GetAllAsync()
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<LkPlantCountry>(_queryProvider.LkPlantCountryQueries.GetAllQuery()); }
            catch (Exception ex) { throw new Exception("Error al obtener la lista de PlantCountry.", ex); }
        }

        public async Task UpdateAsync(LkPlantCountry entity)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.LkPlantCountryQueries.UpdateQuery(), entity);
                if (rows == 0) throw new Exception($"No se encontro PlantCountry para actualizar.");
            }
            catch (Exception ex) { throw new Exception("Error al actualizar PlantCountry.", ex); }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.LkPlantCountryQueries.DeleteQuery(), new { Id = id });
                if (rows == 0) throw new Exception($"No se encontro PlantCountry con id {id} para eliminar.");
            }
            catch (Exception ex) { throw new Exception($"Error al eliminar PlantCountry con id {id}.", ex); }
        }
    }
}
