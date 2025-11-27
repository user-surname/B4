using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.Entities;
using B4.Models.Interfaces;
using B4.Data.MySQL;

namespace B4.Data.MySQL.Repositories
{
    public class DataBridgesFyBwEurRepository : IDataBridgesFyBwEurRepository
    {
        private const string _tableName = "DATA_BRIDGESFY_BW_EUR";
        private readonly MySQLDapperContext _context;

        public DataBridgesFyBwEurRepository(MySQLDapperContext context)
        {
            _context = context;
        }

        // -------------------------------------------------
        // C - CREATE
        // -------------------------------------------------
        public async Task AddAsync(DataBridgesFyBwEur entity)
        {
            const string sql = $@"
                INSERT INTO {_tableName} 
                (idAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                 Actuals, PctActuals, Budget, PctBudget, Variance, Volume, InventoryChange, Mix, `New`, Economics,
                 QuickSavings, CurrencyMix, ExchangeRate, RawMaterial, Scrap, IndustrialPerformance, ProtoTooling, Other,
                 `Check`, Comments, idCarga, idCargaSTGBW, idHoja)
                VALUES
                (@idAPICarga, @guidCarga, @FechaUltModif, @IdCompany, @Ejercicio, @IdCiclo, @IdFase, @IdCurrency, @IdEpigrafe,
                 @Actuals, @PctActuals, @Budget, @PctBudget, @Variance, @Volume, @InventoryChange, @Mix, @New, @Economics,
                 @QuickSavings, @CurrencyMix, @ExchangeRate, @RawMaterial, @Scrap, @IndustrialPerformance, @ProtoTooling, @Other,
                 @Check, @Comments, @idCarga, @idCargaSTGBW, @idHoja)";

            try
            {
                using var conn = _context.CreateConnection();
                await conn.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar un registro en DATA_BRIDGESFY_BW_EUR.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (por ID)
        // -------------------------------------------------
        public async Task<DataBridgesFyBwEur?> GetByIdAsync(int id)
        {
            const string sql = $@"SELECT * FROM {_tableName} WHERE Id = @Id";
            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<DataBridgesFyBwEur>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el registro con Id {id}.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (todos)
        // -------------------------------------------------
        public async Task<IEnumerable<DataBridgesFyBwEur>> GetAllAsync()
        {
            const string sql = $@"SELECT * FROM {_tableName}";
            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QueryAsync<DataBridgesFyBwEur>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener todos los registros de DATA_BRIDGESFY_BW_EUR.", ex);
            }
        }

        // -------------------------------------------------
        // U - UPDATE
        // -------------------------------------------------
        public async Task UpdateAsync(DataBridgesFyBwEur entity)
        {
            const string sql = $@"
                UPDATE {_tableName} SET
                    idAPICarga = @idAPICarga,
                    guidCarga = @guidCarga,
                    FechaUltModif = @FechaUltModif,
                    IdCompany = @IdCompany,
                    Ejercicio = @Ejercicio,
                    IdCiclo = @IdCiclo,
                    IdFase = @IdFase,
                    IdCurrency = @IdCurrency,
                    IdEpigrafe = @IdEpigrafe,
                    Actuals = @Actuals,
                    PctActuals = @PctActuals,
                    Budget = @Budget,
                    PctBudget = @PctBudget,
                    Variance = @Variance,
                    Volume = @Volume,
                    InventoryChange = @InventoryChange,
                    Mix = @Mix,
                    `New` = @New,
                    Economics = @Economics,
                    QuickSavings = @QuickSavings,
                    CurrencyMix = @CurrencyMix,
                    ExchangeRate = @ExchangeRate,
                    RawMaterial = @RawMaterial,
                    Scrap = @Scrap,
                    IndustrialPerformance = @IndustrialPerformance,
                    ProtoTooling = @ProtoTooling,
                    `Other` = @Other,
                    `Check` = @Check,
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
                throw new Exception($"Error al actualizar el registro con Id {entity.Id}.", ex);
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
                throw new Exception($"Error al eliminar el registro con Id {id}.", ex);
            }
        }
    }
}
