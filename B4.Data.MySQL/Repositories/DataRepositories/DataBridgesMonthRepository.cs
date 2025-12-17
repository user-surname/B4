using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.Entities.DataEntities;
using B4.Models.Interfaces.DataInterfaces;

namespace B4.Data.MySQL.Repositories.DataRepositories
{
    public class DataBridgesMonthRepository : DataRepository<DataBridgesMonth>, IDataBridgesMonthRepository
    {
        private const string _tableName = "DATA_BridgesMonth";
        private readonly MySQLDapperContext _context;

        public DataBridgesMonthRepository(MySQLDapperContext context) : base(context, _tableName)
        {
            _context = context;
        }

        // -------------------------------------------------
        // C - CREATE
        // -------------------------------------------------
        public async Task AddAsync(DataBridgesMonth entity)
        {
            const string sql = $@"
                INSERT INTO {_tableName} 
                (idAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                 Actuals, PctActuals, Budget, PctBudget, Variance, Volume, InventoryChange, Mix, `New`,
                 EconomicsInflation, EconomicsInflationClaim, EconomicsPricingIssues, EconomicsLTAS, EconomicsBPs, EconomicsComponentEffect,
                 StockValuation, QuickSavings, CurrencyMix, ExchangeRate, RawMaterial, Scrap,
                 PerformanceGSD, PerformanceQuality, PerformanceOther, PerformanceLaunchingCost,
                 ProtoTooling, AccruralOthers, OneTimeEffectOthers, Other, Comments, checksum, iszero)
                VALUES
                (@idAPICarga, @guidCarga, @FechaUltModif, @IdCompany, @Ejercicio, @IdCiclo, @IdFase, @IdCurrency, @IdEpigrafe,
                 @Actuals, @PctActuals, @Budget, @PctBudget, @Variance, @Volume, @InventoryChange, @Mix, @New,
                 @EconomicsInflation, @EconomicsInflationClaim, @EconomicsPricingIssues, @EconomicsLTAS, @EconomicsBPs, @EconomicsComponentEffect,
                 @StockValuation, @QuickSavings, @CurrencyMix, @ExchangeRate, @RawMaterial, @Scrap,
                 @PerformanceGSD, @PerformanceQuality, @PerformanceOther, @PerformanceLaunchingCost,
                 @ProtoTooling, @AccruralOthers, @OneTimeEffectOthers, @Other, @Comments, @checksum, @iszero)";

            try
            {
                using var conn = _context.CreateConnection();
                await conn.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar un registro en DATA_BridgesMonth.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (por ID)
        // -------------------------------------------------
        //public async Task<DataBridgesMonth?> GetByIdAsync(int id)
        //{
        //    var sql = $@"SELECT * FROM {_tableName} WHERE Id = @Id";

        //    try
        //    {
        //        using var conn = _context.CreateConnection();
        //        return await conn.QuerySingleOrDefaultAsync<DataBridgesMonth>(sql, new { Id = id });
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Error al obtener el registro con Id {id}.", ex);
        //    }
        //}

        //// -------------------------------------------------
        //// R - READ (todos)
        //// -------------------------------------------------
        //public async Task<IEnumerable<DataBridgesMonth>> GetAllAsync()
        //{
        //    var sql = $@"SELECT * FROM {_tableName}";

        //    try
        //    {
        //        using var conn = _context.CreateConnection();
        //        return await conn.QueryAsync<DataBridgesMonth>(sql);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error al obtener todos los registros de DATA_BridgesMonth.", ex);
        //    }
        //}

        // -------------------------------------------------
        // U - UPDATE
        // -------------------------------------------------
        public async Task UpdateAsync(DataBridgesMonth entity)
        {
            var sql = $@"
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
                    EconomicsInflation = @EconomicsInflation,
                    EconomicsInflationClaim = @EconomicsInflationClaim,
                    EconomicsPricingIssues = @EconomicsPricingIssues,
                    EconomicsLTAS = @EconomicsLTAS,
                    EconomicsBPs = @EconomicsBPs,
                    EconomicsComponentEffect = @EconomicsComponentEffect,
                    StockValuation = @StockValuation,
                    QuickSavings = @QuickSavings,
                    CurrencyMix = @CurrencyMix,
                    ExchangeRate = @ExchangeRate,
                    RawMaterial = @RawMaterial,
                    Scrap = @Scrap,
                    PerformanceGSD = @PerformanceGSD,
                    PerformanceQuality = @PerformanceQuality,
                    PerformanceOther = @PerformanceOther,
                    PerformanceLaunchingCost = @PerformanceLaunchingCost,
                    ProtoTooling = @ProtoTooling,
                    AccruralOthers = @AccruralOthers,
                    OneTimeEffectOthers = @OneTimeEffectOthers,
                    Other = @Other,
                    Comments = @Comments,
                    checksum = @checksum,
                    iszero = @iszero
                WHERE Id = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                var rows = await conn.ExecuteAsync(sql, entity);
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
        //public async Task DeleteAsync(int id)
        //{
        //    var sql = $@"DELETE FROM {_tableName} WHERE Id = @Id";

        //    try
        //    {
        //        using var conn = _context.CreateConnection();
        //        var rows = await conn.ExecuteAsync(sql, new { Id = id });
        //        if (rows == 0)
        //            throw new Exception($"No se encontró el registro con Id {id} para eliminar.");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Error al eliminar el registro con Id {id}.", ex);
        //    }
        //}
    }
}

