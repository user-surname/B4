using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.PostgreSQL
{
    public sealed class PostgreSqlDataBridgesMonthBwQueries : IDataBridgesMonthBwQueries
    {
        public string AddQuery() => @"
            INSERT INTO ""b4"".""data_bridges_month_bw""
            (idapicarga, guidcarga, fechaultmodif, idcompany, ejercicio, idciclo, idfase, idcurrency, idepigrafe,
             volume, inventorychange, mix, ""new"", economics, quicksavings, currencymix, exchangerate,
             rawmaterial, scrap, industrialperformance, prototooling, other, ""check"", comments,
             idcarga, idcargastgbw, idhoja)
            VALUES
            (@IdAPICarga, @GuidCarga, @FechaUltModif, @IdCompany, @Ejercicio, @IdCiclo, @IdFase, @IdCurrency, @IdEpigrafe,
             @Volume, @InventoryChange, @Mix, @New, @Economics, @QuickSavings, @CurrencyMix, @ExchangeRate,
             @RawMaterial, @Scrap, @IndustrialPerformance, @ProtoTooling, @Other, @Check, @Comments,
             @IdCarga, @IdCargaSTGBW, @IdHoja)
            RETURNING id";

        public string GetByIdQuery() => @"SELECT * FROM ""b4"".""data_bridges_month_bw"" WHERE id = @Id";

        public string GetAllQuery() => @"SELECT * FROM ""b4"".""data_bridges_month_bw"" LIMIT 2000";

        public string UpdateQuery() => @"
            UPDATE ""b4"".""data_bridges_month_bw"" SET
                idapicarga=@IdAPICarga, guidcarga=@GuidCarga, fechaultmodif=@FechaUltModif,
                idcompany=@IdCompany, ejercicio=@Ejercicio, idciclo=@IdCiclo, idfase=@IdFase,
                idcurrency=@IdCurrency, idepigrafe=@IdEpigrafe,
                volume=@Volume, inventorychange=@InventoryChange, mix=@Mix, ""new""=@New, economics=@Economics,
                quicksavings=@QuickSavings, currencymix=@CurrencyMix, exchangerate=@ExchangeRate,
                rawmaterial=@RawMaterial, scrap=@Scrap, industrialperformance=@IndustrialPerformance,
                prototooling=@ProtoTooling, other=@Other, ""check""=@Check, comments=@Comments,
                idcarga=@IdCarga, idcargastgbw=@IdCargaSTGBW, idhoja=@IdHoja
            WHERE id=@Id";

        public string DeleteQuery() => @"DELETE FROM ""b4"".""data_bridges_month_bw"" WHERE id = @Id";

        public string GetByPlantaEjercicioQuery() => @"
            SELECT * FROM ""b4"".""data_bridges_month_bw""
            WHERE idcompany = @planta AND ejercicio = @ejercicio ORDER BY idepigrafe";
    }
}
