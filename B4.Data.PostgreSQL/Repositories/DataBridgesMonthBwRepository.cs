using Dapper;
using Npgsql;
using B4.Models.Entities.DataEtities;
using B4.Models.Interfaces.DataInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class DataBridgesMonthBwRepository : IDataBridgesMonthBwRepository
    {
        private readonly string _connectionString;

        private const string _table = "\"b4\".\"data_bridges_month_bw\"";

        public DataBridgesMonthBwRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddAsync(DataBridgesMonthBw entity)
        {
            var sql = $@"
                INSERT INTO {_table} (
                    idapicarga, guidcarga, fechaultmodif,
                    idcompany, ejercicio, idciclo, idfase,
                    idcurrency, idepigrafe,
                    volume, inventorychange, mix, new, economics,
                    quicksavings, currencymix, exchangerate,
                    rawmaterial, scrap, industrialperformance,
                    prototooling, other, ""check"", comments,
                    idcarga, idcargastgbw, idhoja
                )
                VALUES (
                    @IdAPICarga, @GuidCarga, @FechaUltModif,
                    @IdCompany, @Ejercicio, @IdCiclo, @IdFase,
                    @IdCurrency, @IdEpigrafe,
                    @Volume, @InventoryChange, @Mix, @New, @Economics,
                    @QuickSavings, @CurrencyMix, @ExchangeRate,
                    @RawMaterial, @Scrap, @IndustrialPerformance,
                    @ProtoTooling, @Other, @Check, @Comments,
                    @IdCarga, @IdCargaSTGBW, @IdHoja
                )
                RETURNING id;
            ";

            using var conn = new NpgsqlConnection(_connectionString);
            entity.Id = await conn.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task<DataBridgesMonthBw?> GetByIdAsync(int id)
        {
            var sql = $@"SELECT * FROM {_table} WHERE id = @Id";
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QuerySingleOrDefaultAsync<DataBridgesMonthBw>(sql, new { Id = id });
        }

        public async Task<IEnumerable<DataBridgesMonthBw>> GetAllAsync()
        {
            var sql = $@"SELECT * FROM {_table}";
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QueryAsync<DataBridgesMonthBw>(sql);
        }

        public async Task UpdateAsync(DataBridgesMonthBw entity)
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
            int rows = await conn.ExecuteAsync(sql, entity);

            if (rows == 0)
                throw new Exception($"No existe registro con ID {entity.Id}");
        }

        public async Task DeleteAsync(int id)
        {
            var sql = $@"DELETE FROM {_table} WHERE id = @Id";
            using var conn = new NpgsqlConnection(_connectionString);
            int rows = await conn.ExecuteAsync(sql, new { Id = id });

            if (rows == 0)
                throw new Exception($"No existe registro con ID {id}");
        }
    }
}
