using Dapper;
using Npgsql;
using B4.Models.Entities.DataEtities;
using B4.Models.Interfaces.DataInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class DataBridgesFyBwRepository : IDataBridgesFyBwRepository
    {
        private readonly string _connectionString;
        private const string _table = "b4.data_bridges_fy_bw";

        public DataBridgesFyBwRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // CREATE
        public async Task AddAsync(DataBridgesFyBw entity)
        {
            var sql = $@"
                INSERT INTO {_table} (
                    idapicarga, guidcarga, fechaultmodif,
                    idcompany, ejercicio, idciclo, idfase,
                    idcurrency, idepigrafe,
                    fiscalyear, percentage, zero, zeropercentage,
                    absolute, absolutepercentage,
                    vmixnew, rawmaterial, scrap, economics,
                    currencymix, performance, prototool, others,
                    comments, idcarga, idcargastgbw, idhoja
                )
                VALUES (
                    @IdAPICarga, @GuidCarga, @FechaUltModif,
                    @IdCompany, @Ejercicio, @IdCiclo, @IdFase,
                    @IdCurrency, @IdEpigrafe,
                    @FiscalYear, @Percentage, @Zero, @ZeroPercentage,
                    @Absolute, @AbsolutePercentage,
                    @VMixNew, @RawMaterial, @Scrap, @Economics,
                    @CurrencyMix, @Performance, @ProtoTool, @Others,
                    @Comments, @IdCarga, @IdCargaSTGBW, @IdHoja
                )
                RETURNING id;
            ";

            using var conn = new NpgsqlConnection(_connectionString);
            var newId = await conn.ExecuteScalarAsync<int>(sql, entity);
            entity.Id = newId;
        }

        // READ
        public async Task<DataBridgesFyBw?> GetByIdAsync(int id)
        {
            var sql = $@"SELECT * FROM {_table} WHERE id = @Id";

            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QuerySingleOrDefaultAsync<DataBridgesFyBw>(sql, new { Id = id });
        }

        public async Task<IEnumerable<DataBridgesFyBw>> GetAllAsync()
        {
            var sql = $@"SELECT * FROM {_table}";
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QueryAsync<DataBridgesFyBw>(sql);
        }

        // UPDATE
        public async Task UpdateAsync(DataBridgesFyBw entity)
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
                    fiscalyear = @FiscalYear,
                    percentage = @Percentage,
                    zero = @Zero,
                    zeropercentage = @ZeroPercentage,
                    absolute = @Absolute,
                    absolutepercentage = @AbsolutePercentage,
                    vmixnew = @VMixNew,
                    rawmaterial = @RawMaterial,
                    scrap = @Scrap,
                    economics = @Economics,
                    currencymix = @CurrencyMix,
                    performance = @Performance,
                    prototool = @ProtoTool,
                    others = @Others,
                    comments = @Comments,
                    idcarga = @IdCarga,
                    idcargastgbw = @IdCargaSTGBW,
                    idhoja = @IdHoja
                WHERE id = @Id;
            ";

            using var conn = new NpgsqlConnection(_connectionString);
            int rows = await conn.ExecuteAsync(sql, entity);

            if (rows == 0)
                throw new Exception($"No existe registro DataBridgesFyBw con id {entity.Id}");
        }

        // DELETE
        public async Task DeleteAsync(int id)
        {
            var sql = $@"DELETE FROM {_table} WHERE id = @Id";

            using var conn = new NpgsqlConnection(_connectionString);
            int rows = await conn.ExecuteAsync(sql, new { Id = id });

            if (rows == 0)
                throw new Exception($"No existe registro DataBridgesFyBw con id {id}");
        }
    }
}
