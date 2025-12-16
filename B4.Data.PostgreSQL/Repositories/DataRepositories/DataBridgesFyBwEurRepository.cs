using Dapper;
using B4.Models.Entities.DataEntities;
using B4.Models.Interfaces.DataInterfaces;

namespace B4.Data.PostgreSQL.Repositories.DataRepositories
{
    public class DataBridgesFyBwEurRepository 
        : BaseRepository<DataBridgesFyBwEur>, IDataBridgesFyBwEurRepository
    {
        public DataBridgesFyBwEurRepository(PostgreSQLDapperContext context)
            : base(context, "b4.data_bridges_fy_bw_eur") { }

        public async Task AddAsync(DataBridgesFyBwEur entity)
        {
            var sql = @"
                INSERT INTO b4.data_bridges_fy_bw_eur
                (idapicarga, guidcarga, fechaultmodif,
                 idcompany, ejercicio, idciclo, idfase,
                 idcurrency, idepigrafe,
                 actuals, pctactuals, budget, pctbudget, variance,
                 volume, inventorychange, mix, new, economics, quicksavings,
                 currencymix, exchangerate, rawmaterial, scrap, industrialperformance,
                 prototooling, other, ""check"", comments,
                 idcarga, idcargastgbw, idhoja)
                VALUES
                (@IdAPICarga, @GuidCarga, @FechaUltModif,
                 @IdCompany, @Ejercicio, @IdCiclo, @IdFase,
                 @IdCurrency, @IdEpigrafe,
                 @Actuals, @PctActuals, @Budget, @PctBudget, @Variance,
                 @Volume, @InventoryChange, @Mix, @New, @Economics, @QuickSavings,
                 @CurrencyMix, @ExchangeRate, @RawMaterial, @Scrap, @IndustrialPerformance,
                 @ProtoTooling, @Other, @Check, @Comments,
                 @IdCarga, @IdCargaSTGBW, @IdHoja)
                RETURNING id;";

            using var conn = GetConnection();
            entity.Id = await conn.ExecuteScalarAsync<int>(sql);
        }

        public async Task UpdateAsync(DataBridgesFyBwEur entity)
        {
            var sql = @"
                UPDATE b4.data_bridges_fy_bw_eur SET
                    idapicarga=@IdAPICarga,
                    guidcarga=@GuidCarga,
                    fechaultmodif=@FechaUltModif,
                    idcompany=@IdCompany,
                    ejercicio=@Ejercicio,
                    idciclo=@IdCiclo,
                    idfase=@IdFase,
                    idcurrency=@IdCurrency,
                    idepigrafe=@IdEpigrafe,
                    actuals=@Actuals,
                    pctactuals=@PctActuals,
                    budget=@Budget,
                    pctbudget=@PctBudget,
                    variance=@Variance,
                    volume=@Volume,
                    inventorychange=@InventoryChange,
                    mix=@Mix,
                    new=@New,
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
                WHERE id=@Id";

            using var conn = GetConnection();
            await conn.ExecuteAsync(sql, entity);
        }
    }
}
