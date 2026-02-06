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
                (idapicarga, guidcarga, fechaultmodif,
                 idcompany, ejercicio, idciclo, idfase,
                 idcurrency, idepigrafe,
                 fiscalyear, percentage, zero, zeropercentage,
                 absolute, absolutepercentage,
                 vmixnew, rawmaterial, scrap, economics,
                 currencymix, performance, prototool, others,
                 comments, idcarga, idcargastgbw, idhoja)
                VALUES
                (@IdAPICarga, @GuidCarga, @FechaUltModif,
                 @IdCompany, @Ejercicio, @IdCiclo, @IdFase,
                 @IdCurrency, @IdEpigrafe,
                 @FiscalYear, @Percentage, @Zero, @ZeroPercentage,
                 @Absolute, @AbsolutePercentage,
                 @VMixNew, @RawMaterial, @Scrap, @Economics,
                 @CurrencyMix, @Performance, @ProtoTool, @Others,
                 @Comments, @IdCarga, @IdCargaSTGBW, @IdHoja)
                RETURNING id;";

            using var conn = GetConnection();
            entity.Id = await conn.ExecuteScalarAsync<int>(sql);
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
                WHERE id=@Id";

            using var conn = GetConnection();
            await conn.ExecuteAsync(sql, entity);
        }
    }
}
