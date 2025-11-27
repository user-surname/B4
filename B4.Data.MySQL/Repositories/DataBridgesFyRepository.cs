using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.Entities;
using B4.Models.Interfaces;
using B4.Data.MySQL;
using System;

namespace B4.Data.MySQL.Repositories
{
    public class DataBridgesFyRepository : IDataBridgesFyRepository
    {
        private const string _tableName = "DATA_BridgesFY";
        private readonly MySQLDapperContext _context;

        public DataBridgesFyRepository(MySQLDapperContext context)
        {
            _context = context;
        }

        // -------------------------------------------------
        // C - CREATE
        // -------------------------------------------------
        public async Task AddAsync(DataBridgesFy entity)
        {
            const string sql = @"
                INSERT INTO DATA_BridgesFY
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
                 @Others, @Comments);
            ";

            try
            {
                using var conn = _context.CreateConnection();
                await conn.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar DataBridgesFY.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (por ID)
        // -------------------------------------------------
        public async Task<DataBridgesFy?> GetByIdAsync(int id)
        {
            const string sql = @"SELECT * FROM DATA_BridgesFY WHERE Id = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<DataBridgesFy>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener DataBridgesFY con Id {id}.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (todos)
        // -------------------------------------------------
        public async Task<IEnumerable<DataBridgesFy>> GetAllAsync()
        {
            const string sql = @"SELECT * FROM DATA_BridgesFY";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QueryAsync<DataBridgesFy>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de DataBridgesFY.", ex);
            }
        }

        // -------------------------------------------------
        // U - UPDATE
        // -------------------------------------------------
        public async Task UpdateAsync(DataBridgesFy entity)
        {
            const string sql = @"
                UPDATE DATA_BridgesFY SET
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
                WHERE Id = @Id;
            ";

            try
            {
                using var conn = _context.CreateConnection();
                var rows = await conn.ExecuteAsync(sql, entity);

                if (rows == 0)
                    throw new Exception($"No se encontró DataBridgesFY con Id {entity.Id} para actualizar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar DataBridgesFY con Id {entity.Id}.", ex);
            }
        }

        // -------------------------------------------------
        // D - DELETE
        // -------------------------------------------------
        public async Task DeleteAsync(int id)
        {
            const string sql = @"DELETE FROM DATA_BridgesFY WHERE Id = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                var rows = await conn.ExecuteAsync(sql, new { Id = id });

                if (rows == 0)
                    throw new Exception($"No se encontró DataBridgesFY con Id {id} para eliminar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar DataBridgesFY con Id {id}.", ex);
            }
        }
    }
}

