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
    public sealed class LkPlantDivisionCompanyRepository : IPlantDivisionCompanyRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        public LkPlantDivisionCompanyRepository(IDbConnectionFactory connectionFactory, IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        public async Task AddAsync(LkPlantDivisionCompany entity)
        {
            try { using var conn = _connectionFactory.CreateConnection(); await conn.ExecuteAsync(_queryProvider.LkPlantDivisionCompanyQueries.AddQuery(), entity); }
            catch (Exception ex) { throw new Exception("Error al insertar PlantDivisionCompany.", ex); }
        }

        public async Task<LkPlantDivisionCompany?> GetByIdAsync(int id)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QuerySingleOrDefaultAsync<LkPlantDivisionCompany>(_queryProvider.LkPlantDivisionCompanyQueries.GetByIdQuery(), new { Id = id }); }
            catch (Exception ex) { throw new Exception($"Error al obtener PlantDivisionCompany con id {id}.", ex); }
        }

        public async Task<IEnumerable<LkPlantDivisionCompany>> GetAllAsync()
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<LkPlantDivisionCompany>(_queryProvider.LkPlantDivisionCompanyQueries.GetAllQuery()); }
            catch (Exception ex) { throw new Exception("Error al obtener la lista de PlantDivisionCompany.", ex); }
        }

        public async Task UpdateAsync(LkPlantDivisionCompany entity)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.LkPlantDivisionCompanyQueries.UpdateQuery(), entity);
                if (rows == 0) throw new Exception($"No se encontro PlantDivisionCompany para actualizar.");
            }
            catch (Exception ex) { throw new Exception("Error al actualizar PlantDivisionCompany.", ex); }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.LkPlantDivisionCompanyQueries.DeleteQuery(), new { Id = id });
                if (rows == 0) throw new Exception($"No se encontro PlantDivisionCompany con id {id} para eliminar.");
            }
            catch (Exception ex) { throw new Exception($"Error al eliminar PlantDivisionCompany con id {id}.", ex); }
        }
    }
}
