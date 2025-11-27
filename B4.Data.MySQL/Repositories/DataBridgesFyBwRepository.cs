using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.Entities;
using B4.Models.Interfaces;
using B4.Data.MySQL;
using System;

namespace B4.Data.MySQL.Repositories
{
    public class DataBridgesFyBwRepository : IDataBridgesFyBwRepository
    {
        // Nombre de la tabla
        private const string _tableName = "DATA_BridgesFY_BW";

        private readonly MySQLDapperContext _context;

        public DataBridgesFyBwRepository(MySQLDapperContext context)
        {
            _context = context;
        }

        // -------------------------------------------------
        // C - CREATE
        // -------------------------------------------------
        public async Task AddAsync(DataBridgesFyBw entity)
        {
            const string sql = $@"
                INSERT INTO {_tableName}
                (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                 FiscalYear, Percentage, Zero, ZeroPercentage, Absolute, AbsolutePercentage, VMixNew, RawMaterial, Scrap, Economics,
                 CurrencyMix, Performance, ProtoTool, Others, Comments, idCarga, idCargaSTGBW, idHoja)
                VALUES
                (@IdAPICarga, @guidCarga, @FechaUltModif, @IdCompany, @Ejercicio, @IdCiclo, @IdFase, @IdCurrency, @IdEpigrafe,
                 @FiscalYear, @Percentage, @Zero, @ZeroPercentage, @Absolute, @AbsolutePercentage, @VMixNew, @RawMaterial, @Scrap, @Economics,
                 @CurrencyMix, @Performance, @ProtoTool, @Others, @Comments, @idCarga, @idCargaSTGBW, @idHoja)";

            try
            {
                using var conn = _context.CreateConnection();
                await conn.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar un registro en DATA_BridgesFY_BW.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (por ID)
        // -------------------------------------------------
        public async Task<DataBridgesFyBw?> GetByIdAsync(int id)
        {
            const string sql = $@"SELECT * FROM {_tableName} WHERE Id = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<DataBridgesFyBw>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el registro con Id {id} en DATA_BridgesFY_BW.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (todos)
        // -------------------------------------------------
        public async Task<IEnumerable<DataBridgesFyBw>> GetAllAsync()
        {
            const string sql = $@"SELECT * FROM {_tableName}";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QueryAsync<DataBridgesFyBw>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de registros en DATA_BridgesFY_BW.", ex);
            }
        }

        // -------------------------------------------------
        // U - UPDATE
        // -------------------------------------------------
        public async Task UpdateAsync(DataBridgesFyBw entity)
        {
            const string sql = $@"
                UPDATE {_tableName} SET
                    IdAPICarga = @IdAPICarga,
                    guidCarga = @guidCarga,
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
                    Comments = @Comments,
                    idCarga = @idCarga,
                    idCargaSTGBW = @idCargaSTGBW,
                    idHoja = @idHoja
                WHERE Id = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                int rows = await conn.ExecuteAsync(sql, entity);

                if (rows == 0)
                    throw new Exception($"No se encontró el registro con Id {entity.Id} para actualizar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el registro con Id {entity.Id} en DATA_BridgesFY_BW.", ex);
            }
        }

        // -------------------------------------------------
        // D - DELETE
        // -------------------------------------------------
        public async Task DeleteAsync(int id)
        {
            const string sql = $@"DELETE FROM {_tableName} WHERE Id = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                int rows = await conn.ExecuteAsync(sql, new { Id = id });

                if (rows == 0)
                    throw new Exception($"No se encontró el registro con Id {id} para eliminar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el registro con Id {id} en DATA_BridgesFY_BW.", ex);
            }
        }
    }
}
