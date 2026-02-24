using Dapper;
using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces;

namespace B4.Data.PostgreSQL.Repositories.DataRepositories
{
    public class DataBridgesFyBwRepository
        : BaseRepository<DataBridgesFyBw>, IDataBridgesFyBwRepository
    {
        public DataBridgesFyBwRepository(PostgreSQLDapperContext context)
            : base(context, "b4.data_bridges_fy_bw") { }

        public async Task AddAsync(DataBridgesFyBw entity)
        {
            var sql = @"
                INSERT INTO b4.data_bridges_fy_bw
                (
                    idapicarga, guidcarga, fechaultmodif,
                    idcompany, ejercicio, idciclo, idfase,
                    idcurrency, idepigrafe,
                    fiscalyear, percentage, zero, zeropercentage,
                    absolute, absolutepercentage,
                    vmixnew, rawmaterial, scrap, economics,
                    currencymix, performance, prototool, others,
                    comments, idcarga, idcargastgbw, idhoja
                )
                VALUES
                (
                    @IdAPICarga, @GuidCarga, @FechaUltModif,
                    @IdCompany, @Ejercicio, @IdCiclo, @IdFase,
                    @IdCurrency, @IdEpigrafe,
                    @FiscalYear, @Percentage, @Zero, @ZeroPercentage,
                    @Absolute, @AbsolutePercentage,
                    @VMixNew, @RawMaterial, @Scrap, @Economics,
                    @CurrencyMix, @Performance, @ProtoTool, @Others,
                    @Comments, @IdCarga, @IdCargaSTGBW, @IdHoja
                )
                RETURNING id;";

            using var conn = GetConnection();

            // ✅ FIX: pasar entity como parámetros
            entity.Id = await conn.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task UpdateAsync(DataBridgesFyBw entity)
        {
            var sql = @"
                UPDATE b4.data_bridges_fy_bw SET
                    idapicarga=@IdAPICarga,
                    guidcarga=@GuidCarga,
                    fechaultmodif=@FechaUltModif,
                    idcompany=@IdCompany,
                    ejercicio=@Ejercicio,
                    idciclo=@IdCiclo,
                    idfase=@IdFase,
                    idcurrency=@IdCurrency,
                    idepigrafe=@IdEpigrafe,
                    fiscalyear=@FiscalYear,
                    percentage=@Percentage,
                    zero=@Zero,
                    zeropercentage=@ZeroPercentage,
                    absolute=@Absolute,
                    absolutepercentage=@AbsolutePercentage,
                    vmixnew=@VMixNew,
                    rawmaterial=@RawMaterial,
                    scrap=@Scrap,
                    economics=@Economics,
                    currencymix=@CurrencyMix,
                    performance=@Performance,
                    prototool=@ProtoTool,
                    others=@Others,
                    comments=@Comments,
                    idcarga=@IdCarga,
                    idcargastgbw=@IdCargaSTGBW,
                    idhoja=@IdHoja
                WHERE id=@Id;";

            using var conn = GetConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        // ✅ AÑADIDO: GET explícito (evita problemas del BaseRepository con schema/columnas)
        public async Task<DataBridgesFyBw?> GetByIdAsync(int id)
        {
            var sql = @"
                SELECT
                    id,
                    idapicarga         AS ""IdAPICarga"",
                    guidcarga          AS ""GuidCarga"",
                    fechaultmodif      AS ""FechaUltModif"",
                    idcompany          AS ""IdCompany"",
                    ejercicio          AS ""Ejercicio"",
                    idciclo            AS ""IdCiclo"",
                    idfase             AS ""IdFase"",
                    idcurrency         AS ""IdCurrency"",
                    idepigrafe         AS ""IdEpigrafe"",

                    fiscalyear         AS ""FiscalYear"",
                    percentage         AS ""Percentage"",
                    zero               AS ""Zero"",
                    zeropercentage     AS ""ZeroPercentage"",
                    absolute           AS ""Absolute"",
                    absolutepercentage AS ""AbsolutePercentage"",
                    vmixnew            AS ""VMixNew"",
                    rawmaterial        AS ""RawMaterial"",
                    scrap              AS ""Scrap"",
                    economics          AS ""Economics"",
                    currencymix        AS ""CurrencyMix"",
                    performance        AS ""Performance"",
                    prototool          AS ""ProtoTool"",
                    others             AS ""Others"",

                    comments           AS ""Comments"",
                    idcarga            AS ""IdCarga"",
                    idcargastgbw       AS ""IdCargaSTGBW"",
                    idhoja             AS ""IdHoja"",

                    createdat,
                    updatedat,
                    version,
                    checksum,
                    iszero
                FROM b4.data_bridges_fy_bw
                WHERE id = @id;";

            using var conn = GetConnection();
            return await conn.QuerySingleOrDefaultAsync<DataBridgesFyBw>(sql, new { id });
        }
    }
}