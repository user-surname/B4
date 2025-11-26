using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.Entities;
using B4.Models.Interfaces;
using B4.Data.MySQL;

namespace B4.Data.MySQL.Repositories
{
    public class StgDataBridgesFyRepository : IStgDataBridgesFyRepository
    {
        private const string _tableName = "STG_DATA_BridgesFY";
        private readonly DapperContext _context;

        public StgDataBridgesFyRepository(DapperContext context)
        {
            _context = context;
        }

        // -------------------------------------------------
        // C - CREATE
        // -------------------------------------------------
        public async Task AddAsync(StgDataBridgesFy entity)
        {
            var sql = $@"
                INSERT INTO {_tableName}
                (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio,
                 IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                 FiscalYear, Percentage, Zero, ZeroPercentage, Absolute, AbsolutePercentage,
                 VMixNew, RawMaterial, Scrap, Economics, CurrencyMix, Performance, ProtoTool,
                 Others, Comments)
                VALUES
                (@IdAPICarga, @GuidCarga, @FechaUltModif, @IdCompany, @Ejercicio,
                 @IdCiclo, @IdFase, @IdCurrency, @IdEpigrafe,
                 @FiscalYear, @Percentage, @Zero, @ZeroPercentage, @Absolute, @AbsolutePercentage,
                 @VMixNew, @RawMaterial, @Scrap, @Economics, @CurrencyMix, @Performance, @ProtoTool,
                 @Others, @Comments);";

            try
            {
                using var conn = _context.CreateConnection();
                await conn.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar StgDataBridgesFy.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (por ID)
        // -------------------------------------------------
        public async Task<StgDataBridgesFy?> GetByIdAsync(int id)
        {
            var sql = $@"SELECT * FROM {_tableName} WHERE Id = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<StgDataBridgesFy>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener StgDataBridgesFy con Id {id}.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (todos)
        // -------------------------------------------------
        public async Task<IEnumerable<StgDataBridgesFy>> GetAllAsync()
        {
            var sql = $@"SELECT * FROM {_tableName}";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QueryAsync<StgDataBridgesFy>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de StgDataBridgesFy.", ex);
            }
        }

        // -------------------------------------------------
        // U - UPDATE
        // -------------------------------------------------
        public async Task UpdateAsync(StgDataBridgesFy entity)
        {
            var sql = $@"
                UPDATE {_tableName} SET
                    IdAPICarga = @IdAPICarga,
                    guidCarga = @GuidCarga,
                    FechaUltModif = @FechaUltModif,
                    IdCompany = @IdCompany,
                    Ejercicio = @Ejercicio,
                    IdCiclo = @IdCiclo,
                    IdFase = @IdFase,
                    IdCurrency = @IdCurrency,
                    IdEpigrafe = @IdEpigrafe,
                    FiscalYear = @FiscalYear,
                    Percentage = @Percentage,
                    Zero = @Zero,
                    ZeroPercentage = @ZeroPercentage,
                    Absolute = @Absolute,
                    AbsolutePercentage = @AbsolutePercentage,
                    VMixNew = @VMixNew,
                    RawMaterial = @RawMaterial,
                    Scrap = @Scrap,
                    Economics = @Economics,
                    CurrencyMix = @CurrencyMix,
                    Performance = @Performance,
                    ProtoTool = @ProtoTool,
                    Others = @Others,
                    Comments = @Comments
                WHERE Id = @Id;";

            try
            {
                using var conn = _context.CreateConnection();
                var rows = await conn.ExecuteAsync(sql, entity);

                if (rows == 0)
                    throw new Exception($"No se encontró StgDataBridgesFy con Id {entity.Id} para actualizar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar StgDataBridgesFy con Id {entity.Id}.", ex);
            }
        }

        // -------------------------------------------------
        // D - DELETE
        // -------------------------------------------------
        public async Task DeleteAsync(int id)
        {
            var sql = $@"DELETE FROM {_tableName} WHERE Id = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                var rows = await conn.ExecuteAsync(sql, new { Id = id });

                if (rows == 0)
                    throw new Exception($"No se encontró StgDataBridgesFy con Id {id} para eliminar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar StgDataBridgesFy con Id {id}.", ex);
            }
        }
    }
}

