using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using B4.Models.Entities;
using B4.Models.Interfaces;
using B4.Data.MySQL;
using System.Data;

namespace B4.Data.MySQL.Repositories
{
    public class EpigrafeRepository : IEpigrafeRepository
    {
        private readonly DapperContext _context;
        private const string _tableName = "LK_EPIGRAFES";
        public EpigrafeRepository(DapperContext context)
        {
            _context = context;
        }

        // C - CREATE
        public async Task AddAsync(Epigrafe entity)
        {
            const string sql = $@"
                INSERT INTO {_tableName} 
                (idEpigrafe, idPlantilla, idHoja, PreEpigrafe, Epigrafe, EpigrafeFull)
                VALUES (@IdEpigrafe, @IdPlantilla, @IdHoja, @PreEpigrafe, @EpigrafeX, @EpigrafeFull)";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        // R - READ
        public async Task<Epigrafe?> GetByIdAsync(int id)
        {
            const string sql = $@"SELECT * FROM {_tableName} WHERE idEpigrafe = @Id";

            using var conn = _context.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<Epigrafe>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Epigrafe>> GetAllAsync()
        {
            const string sql = $@"SELECT * FROM {_tableName}";

            using var conn = _context.CreateConnection();
            return await conn.QueryAsync<Epigrafe>(sql);
        }

        // U - UPDATE
        public async Task UpdateAsync(Epigrafe entity)
        {

            const string sql = $@"
                UPDATE {_tableName} SET 
                    idPlantilla = @IdPlantilla,
                    idHoja = @IdHoja,
                    PreEpigrafe = @PreEpigrafe,
                    Epigrafe = @EpigrafeX,
                    EpigrafeFull = @EpigrafeFull
                WHERE idEpigrafe = @IdEpigrafe";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        // D - DELETE
        public async Task DeleteAsync(int id)
        {
            const string sql = $@"DELETE FROM {_tableName} WHERE idEpigrafe = @Id";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, new { Id = id });
        }

        // Extra
        //public async Task<IEnumerable<Epigrafe>> GetByPlantillaAsync(int idPlantilla)
        //{
        //    const string sql = $@"SELECT * FROM {_tableName} WHERE idPlantilla = @idPlantilla";

        //    using var conn = _context.CreateConnection();
        //    return await conn.QueryAsync<Epigrafe>(sql, new { idPlantilla });
        //}


    }
}
