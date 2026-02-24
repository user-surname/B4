using Dapper;
using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces;

namespace B4.Data.PostgreSQL.Repositories.DataRepositories
{
    public class DataBridgesMonthBwRepository
        : BaseRepository<DataBridgesMonthBw>, IDataBridgesMonthBwRepository
    {
        public DataBridgesMonthBwRepository(PostgreSQLDapperContext context)
            : base(context, "\"b4\".\"data_bridges_month_bw\"") { }

        public async Task AddAsync(DataBridgesMonthBw entity)
        {
            var sql = @"
                INSERT INTO ""b4"".""data_bridges_month_bw"" (
                    idapicarga, guidcarga, fechaultmodif,
                    idcompany, ejercicio, idciclo, idfase,
                    idcurrency, idepigrafe,
                    volume, inventorychange, mix, ""new"", economics,
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
                RETURNING id;";

            using var conn = GetConnection();
            // ✅ FIX: pasar entity
            entity.Id = await conn.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task UpdateAsync(DataBridgesMonthBw entity)
        {
            var sql = @"
                UPDATE ""b4"".""data_bridges_month_bw"" SET
                    idapicarga=@IdAPICarga,
                    guidcarga=@GuidCarga,
                    fechaultmodif=@FechaUltModif,
                    idcompany=@IdCompany,
                    ejercicio=@Ejercicio,
                    idciclo=@IdCiclo,
                    idfase=@IdFase,
                    idcurrency=@IdCurrency,
                    idepigrafe=@IdEpigrafe,
                    volume=@Volume,
                    inventorychange=@InventoryChange,
                    mix=@Mix,
                    ""new""=@New,
                    economics=@Economics,
                    quicksavings=@QuickSavings,
                    currencymix=@CurrencyMix,
                    exchangerate=@ExchangeRate,
                    rawmaterial=@RawMaterial,
                    scrap=@Scrap,
                    industrialperformance=@IndustrialPerformance,
                    prototooling=@ProtoTooling,
                    other=@Other,
                    ""check""=@Check,
                    comments=@Comments,
                    idcarga=@IdCarga,
                    idcargastgbw=@IdCargaSTGBW,
                    idhoja=@IdHoja
                WHERE id=@Id;";

            using var conn = GetConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        // ✅ AÑADIDO: SELECT con alias para "check" (y "new" por seguridad)
        public async Task<DataBridgesMonthBw?> GetByIdAsync(int id)
        {
            var sql = @"
                SELECT
                    id,
                    idapicarga,
                    guidcarga,
                    fechaultmodif,
                    idcompany,
                    ejercicio,
                    idciclo,
                    idfase,
                    idcurrency,
                    idepigrafe,
                    volume,
                    inventorychange,
                    mix,
                    ""new"" AS ""New"",
                    economics,
                    quicksavings,
                    currencymix,
                    exchangerate,
                    rawmaterial,
                    scrap,
                    industrialperformance,
                    prototooling,
                    other,
                    ""check"" AS ""Check"",
                    comments,
                    idcarga,
                    idcargastgbw,
                    idhoja,
                    createdat,
                    updatedat,
                    version,
                    checksum,
                    iszero
                FROM ""b4"".""data_bridges_month_bw""
                WHERE id = @id;";

            using var conn = GetConnection();
            return await conn.QuerySingleOrDefaultAsync<DataBridgesMonthBw>(sql, new { id });
        }
    }
}