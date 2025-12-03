using B4.Data.MySQL;
using B4.Models.Entities;
using B4.Models.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace B4.Data.MySQL.Repositories
{
    public abstract class DataBwBaseRepository<T> : IDataBwBaseRepository<T> where T : DataBwBase
    {
        protected readonly MySQLDapperContext _context;
        protected readonly string _tableName;

        protected DataBwBaseRepository(MySQLDapperContext context, string tableName)
        {
            _context = context;
            _tableName = tableName;
        }

        // -------------------------
        // CREATE
        // -------------------------
        public async Task AddAsync(T entity)
        {
            var sql = $@"
                INSERT INTO {_tableName}
                (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio, idCiclo, idFase, idCurrency,
                 idEpigrafe, mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10,
                 mes11, mes12, mes13, idCarga, idCargaSTGBW, idHoja)
                VALUES
                (@IdAPICarga, @GuidCarga, @FechaUltModif, @IdCompany, @Ejercicio, @IdCiclo, @IdFase, @IdCurrency,
                 @IdEpigrafe, @Mes00, @Mes01, @Mes02, @Mes03, @Mes04, @Mes05, @Mes06, @Mes07, @Mes08, @Mes09,
                 @Mes10, @Mes11, @Mes12, @Mes13, @IdCarga, @IdCargaSTGBW, @IdHoja)";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        // -------------------------
        // READ BY ID
        // -------------------------
        public async Task<T?> GetByIdAsync(int id)
        {
            var sql = $@"SELECT * FROM {_tableName} WHERE id = @Id";
            using var conn = _context.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<T>(sql, new { Id = id });
        }

        // -------------------------
        // READ ALL
        // -------------------------
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var sql = $@"SELECT * FROM {_tableName}";
            using var conn = _context.CreateConnection();
            return await conn.QueryAsync<T>(sql);
        }

        // -------------------------
        // UPDATE
        // -------------------------
        public async Task UpdateAsync(T entity)
        {
            var sql = $@"
                UPDATE {_tableName} SET
                    idAPICarga = @IdAPICarga,
                    guidCarga = @GuidCarga,
                    FechaUltModif = @FechaUltModif,
                    idCompany = @IdCompany,
                    ejercicio = @Ejercicio,
                    idCiclo = @IdCiclo,
                    idFase = @IdFase,
                    idCurrency = @IdCurrency,
                    idEpigrafe = @IdEpigrafe,
                    mes00 = @Mes00, mes01 = @Mes01, mes02 = @Mes02, mes03 = @Mes03, mes04 = @Mes04,
                    mes05 = @Mes05, mes06 = @Mes06, mes07 = @Mes07, mes08 = @Mes08, mes09 = @Mes09,
                    mes10 = @Mes10, mes11 = @Mes11, mes12 = @Mes12, mes13 = @Mes13,
                    idCarga = @IdCarga,
                    idCargaSTGBW = @IdCargaSTGBW,
                    idHoja = @IdHoja
                WHERE id = @Id";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        // -------------------------
        // DELETE
        // -------------------------
        public async Task DeleteAsync(int id)
        {
            var sql = $@"DELETE FROM {_tableName} WHERE id = @Id";
            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}

