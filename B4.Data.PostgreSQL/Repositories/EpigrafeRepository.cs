using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class EpigrafeRepository : IEpigrafeRepository
    {
        private const string _tableName = "lk_epigrafe";

        private readonly string _connectionString;

        public EpigrafeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // -------------------------------------------------
        // C - CREATE
        // -------------------------------------------------
        public async Task AddAsync(LkEpigrafe entity)
        {
            const string sql = @"
                INSERT INTO lk_epigrafe 
                (idplantilla, idhoja, preepigrafe, epigrafe, epigrafefull)
                VALUES (@IdPlantilla, @IdHoja, @PreEpigrafe, @Epigrafe, @EpigrafeFull)";

            using var conn = new NpgsqlConnection(_connectionString);
            await conn.ExecuteAsync(sql, entity);
        }

        // -------------------------------------------------
        // R - READ por ID
        // -------------------------------------------------
        public async Task<LkEpigrafe?> GetByIdAsync(int id)
        {
            const string sql = @"SELECT * FROM lk_epigrafe WHERE idepigrafe = @Id";

            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QuerySingleOrDefaultAsync<LkEpigrafe>(sql, new { Id = id });
        }

        // -------------------------------------------------
        // R - READ todos
        // -------------------------------------------------
        public async Task<IEnumerable<LkEpigrafe>> GetAllAsync()
        {
            const string sql = @"SELECT * FROM lk_epigrafe";

            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QueryAsync<LkEpigrafe>(sql);
        }

        // -------------------------------------------------
        // U - UPDATE
        // -------------------------------------------------
        public async Task UpdateAsync(LkEpigrafe entity)
        {
            const string sql = @"
                UPDATE lk_epigrafe SET
                    idplantilla = @IdPlantilla,
                    idhoja = @IdHoja,
                    preepigrafe = @PreEpigrafe,
                    epigrafe = @Epigrafe,
                    epigrafefull = @EpigrafeFull
                WHERE idepigrafe = @IdEpigrafe";

            using var conn = new NpgsqlConnection(_connectionString);
            int rows = await conn.ExecuteAsync(sql, entity);

            if (rows == 0)
                throw new Exception($"No se encontró el Epígrafe con id {entity.IdEpigrafe}");
        }

        // -------------------------------------------------
        // D - DELETE
        // -------------------------------------------------
        public async Task DeleteAsync(int id)
        {
            const string sql = @"DELETE FROM lk_epigrafe WHERE idepigrafe = @Id";

            using var conn = new NpgsqlConnection(_connectionString);
            int rows = await conn.ExecuteAsync(sql, new { Id = id });

            if (rows == 0)
                throw new Exception($"No se encontró el Epígrafe con id {id}");
        }
    }
}
