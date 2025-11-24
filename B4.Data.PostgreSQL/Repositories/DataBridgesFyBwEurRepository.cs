using Dapper;
using Npgsql;
using B4.Models.Entities;
using B4.Models.Interfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class DataBridgesFyBwEurRepository : IDataBridgesFyBwEurRepository
    {
        private readonly string _connectionString;
        private const string _table = "b4.data_bridges_fy_bw_eur";

        public DataBridgesFyBwEurRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // CREATE
        public async Task AddAsync(DataBridgesFyBwEur entity)
        {
            var sql = $@"
                INSERT INTO {_table} (
                    idapicarga, guidcarga, fechaultmodif,
                    idcompany, ejercicio, idciclo, idfase,
                    idcurrency, idepigrafe,
                    actuals, pctactuals, budget, pctbudget, variance,
                    volume, inventorychange, mix, new, economics, quicksavings,
                    currencymix, exchangerate, rawmaterial, scrap, industrialperformance,
                    prototooling, other, ""check"", comments,
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
            entity.Id = await conn.ExecuteScalarAsync<int>(sql, entity);
        }

        // READ BY ID
        public async Task<DataBridgesFyBwEur?> GetByIdAsync(int id)
        {
            var sql = $@"SELECT * FROM {_table} WHERE id = @Id";

            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QuerySingleOrDefaultAsync<DataBridgesFyBwEur>(sql, new { Id = id });
        }

        // READ ALL
        public async Task<IEnumerable<DataBridgesFyBwEur>> GetAllAsync()
        {
            var sql = $@"SELECT * FROM {_table}";

            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QueryAsync<DataBridgesFyBwEur>(sql);
        }

        // UPDATE
        public async Task UpdateAsync(DataBridgesFyBwEur entity)
        {
            var sql = $@"
                UPDATE {_table} SET
                    idapicarga = @IdAPICarga,
                    guidcarga = @GuidCarga,
                    fechaultmodif = @FechaUltModif,
                    idcompany = @IdCompany,
                    ejercicio = @Ejercicio,
                    idciclo = @IdCiclo,
                    idfase = @IdFase,
                    idcurrency = @IdCurrency,
                    idepigrafe = @IdEpigrafe,
                    actuals = @Actuals,
                    pctactuals = @PctActuals,
                    budget = @Budget,
                    pctbudget = @PctBudget,
                    variance = @Variance,
                    volume = @Volume,
                    inventorychange = @InventoryChange,
                    mix = @Mix,
                    new = @New,
                    economics = @Economics,
                    quicksavings = @QuickSavings,
                    currencymix = @CurrencyMix,
                    exchangerate = @ExchangeRate,
                    rawmaterial = @RawMaterial,
                    scrap = @Scrap,
                    industrialperformance = @IndustrialPerformance,
                    prototooling = @ProtoTooling,
                    other = @Other,
                    ""check"" = @Check,
                    comments = @Comments,
                    idcarga = @IdCarga,
                    idcargastgbw = @IdCargaSTGBW,
                    idhoja = @IdHoja
                WHERE id = @Id;
            ";

            using var conn = new NpgsqlConnection(_connectionString);
            var affected = await conn.ExecuteAsync(sql, entity);

            if (affected == 0)
                throw new Exception($"No existe registro DataBridgesFyBwEur con ID {entity.Id}");
        }

        // DELETE
        public async Task DeleteAsync(int id)
        {
            var sql = $@"DELETE FROM {_table} WHERE id = @Id";

            using var conn = new NpgsqlConnection(_connectionString);
            var affected = await conn.ExecuteAsync(sql, new { Id = id });

            if (affected == 0)
                throw new Exception($"No existe registro DataBridgesFyBwEur con ID {id}");
        }
    }
}
