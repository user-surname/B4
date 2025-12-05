using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using B4.Models.Entities.DataEtities;
using B4.Models.Interfaces.DataInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class DataBridgesFyRepository : IDataBridgesFyRepository
    {
        private readonly string _connectionString;
        private const string _table = "b4.data_bridges_fy";   // ⚠️ Asegúrate que la tabla se llama así en PostgreSQL

        public DataBridgesFyRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // ------------------------------------
        // C - CREATE
        // ------------------------------------
        public async Task AddAsync(DataBridgesFy entity)
        {
            var sql = $@"
                INSERT INTO {_table} (
                    idapicarga, guidcarga, fechaultmodif,
                    idcompany, ejercicio, idciclo, idfase,
                    idcurrency, idepigrafe,
                    actuals, pctactuals, budget, pctbudget, variance,
                    volume, inventorychange, mix, new, economics, quicksavings,
                    currencymix, exchangerate, rawmaterial, scrap, industrialperformance,
                    prototooling, ""other"", ""check"", comments,
                    idcarga, idcargastgbw, idhoja
                )
                VALUES (
                    @IdAPICarga, @GuidCarga, @FechaUltModif,
                    @IdCompany, @Ejercicio, @IdCiclo, @IdFase,
                    @IdCurrency, @IdEpigrafe,
                    @Actuals, @PctActuals, @Budget, @PctBudget, @Variance,
                    @Volume, @InventoryChange, @Mix, @New, @Economics, @QuickSavings,
                    @CurrencyMix, @ExchangeRate, @RawMaterial, @Scrap, @IndustrialPerformance,
                    @ProtoTooling, @Other, @Check, @Comments,
                    @IdCarga, @IdCargaSTGBW, @IdHoja
                )
                RETURNING id;
            ";

            using var conn = new NpgsqlConnection(_connectionString);
            var newId = await conn.ExecuteScalarAsync<int>(sql, entity);
            entity.Id = newId;
        }

        // ------------------------------------
        // R - READ (by Id)
        // ------------------------------------
        public async Task<DataBridgesFy?> GetByIdAsync(int id)
        {
            var sql = $@"SELECT * FROM {_table} WHERE id = @Id";

            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QuerySingleOrDefaultAsync<DataBridgesFy>(sql, new { Id = id });
        }

        // ------------------------------------
        // R - READ (all)
        // ------------------------------------
        public async Task<IEnumerable<DataBridgesFy>> GetAllAsync()
        {
            var sql = $@"SELECT * FROM {_table}";

            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QueryAsync<DataBridgesFy>(sql);
        }

        // ------------------------------------
        // U - UPDATE
        // ------------------------------------
        public async Task UpdateAsync(DataBridgesFy entity)
        {
            var sql = $@"
                UPDATE {_table} SET
                    idapicarga         = @IdAPICarga,
                    guidcarga          = @GuidCarga,
                    fechaultmodif      = @FechaUltModif,
                    idcompany          = @IdCompany,
                    ejercicio          = @Ejercicio,
                    idciclo            = @IdCiclo,
                    idfase             = @IdFase,
                    idcurrency         = @IdCurrency,
                    idepigrafe         = @IdEpigrafe,
                    actuals            = @Actuals,
                    pctactuals         = @PctActuals,
                    budget             = @Budget,
                    pctbudget          = @PctBudget,
                    variance           = @Variance,
                    volume             = @Volume,
                    inventorychange    = @InventoryChange,
                    mix                = @Mix,
                    new                = @New,
                    economics          = @Economics,
                    quicksavings       = @QuickSavings,
                    currencymix        = @CurrencyMix,
                    exchangerate       = @ExchangeRate,
                    rawmaterial        = @RawMaterial,
                    scrap              = @Scrap,
                    industrialperformance = @IndustrialPerformance,
                    prototooling       = @ProtoTooling,
                    ""other""           = @Other,
                    ""check""           = @Check,
                    comments           = @Comments,
                    idcarga            = @IdCarga,
                    idcargastgbw       = @IdCargaSTGBW,
                    idhoja             = @IdHoja
                WHERE id = @Id;
            ";

            using var conn = new NpgsqlConnection(_connectionString);
            int affected = await conn.ExecuteAsync(sql, entity);

            if (affected == 0)
                throw new Exception($"No existe DataBridgesFy con ID {entity.Id}");
        }

        // ------------------------------------
        // D - DELETE
        // ------------------------------------
        public async Task DeleteAsync(int id)
        {
            var sql = $@"DELETE FROM {_table} WHERE id = @Id";

            using var conn = new NpgsqlConnection(_connectionString);
            int affected = await conn.ExecuteAsync(sql, new { Id = id });

            if (affected == 0)
                throw new Exception($"No existe DataBridgesFy con ID {id}");
        }
    }
}
