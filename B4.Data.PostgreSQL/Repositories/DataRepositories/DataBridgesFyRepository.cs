using Dapper;
using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces;

namespace B4.Data.PostgreSQL.Repositories.DataRepositories
{
    public class DataBridgesFyRepository
        : BaseRepository<DataBridgesFy>, IDataBridgesFyRepository
    {
        public DataBridgesFyRepository(PostgreSQLDapperContext context)
            : base(context, "b4.data_bridges_fy") { }

        public async Task AddAsync(DataBridgesFy entity)
        {
            var sql = @"
                INSERT INTO b4.data_bridges_fy
                (
                    idapicarga, guidcarga, fechaultmodif,
                    idcompany, ejercicio, idciclo, idfase,
                    idcurrency, idepigrafe,

                    fiscalyear, percentage, zero, zeropercentage,
                    absolute, absolutepercentage, vmixnew,
                    rawmaterial, scrap, economics, currencymix,
                    performance, prototool, others, comments
                )
                VALUES
                (
                    @IdAPICarga, @GuidCarga, @FechaUltModif,
                    @IdCompany, @Ejercicio, @IdCiclo, @IdFase,
                    @IdCurrency, @IdEpigrafe,

                    @FiscalYear, @Percentage, @Zero, @ZeroPercentage,
                    @Absolute, @AbsolutePercentage, @VMixNew,
                    @RawMaterial, @Scrap, @Economics, @CurrencyMix,
                    @Performance, @ProtoTool, @Others, @Comments
                )
                RETURNING id;";

            using var conn = GetConnection();
            entity.Id = await conn.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task UpdateAsync(DataBridgesFy entity)
        {
            var sql = @"
                UPDATE b4.data_bridges_fy SET
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
                    comments = @Comments
                WHERE id = @Id;";

            using var conn = GetConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        // (Opcional, pero útil si tu BaseRepository genera SELECT raro)
        public async Task<DataBridgesFy?> GetByIdAsync(int id)
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

                    fiscalyear      AS ""FiscalYear"",
                    percentage      AS ""Percentage"",
                    zero            AS ""Zero"",
                    zeropercentage  AS ""ZeroPercentage"",
                    absolute        AS ""Absolute"",
                    absolutepercentage AS ""AbsolutePercentage"",
                    vmixnew         AS ""VMixNew"",
                    rawmaterial     AS ""RawMaterial"",
                    scrap           AS ""Scrap"",
                    economics       AS ""Economics"",
                    currencymix     AS ""CurrencyMix"",
                    performance     AS ""Performance"",
                    prototool       AS ""ProtoTool"",
                    others          AS ""Others"",
                    comments        AS ""Comments"",

                    createdat,
                    updatedat,
                    version,
                    checksum,
                    iszero
                FROM b4.data_bridges_fy
                WHERE id = @id;";

            using var conn = GetConnection();
            return await conn.QuerySingleOrDefaultAsync<DataBridgesFy>(sql, new { id });
        }
    }
}