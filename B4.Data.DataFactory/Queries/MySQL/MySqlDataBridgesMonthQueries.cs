using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.MySQL
{
    public sealed class MySqlDataBridgesMonthQueries : IDataBridgesMonthQueries
    {
        public string AddQuery() => @"
            INSERT INTO DATA_BridgesMonth
            (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
             Volume, InventoryChange, Mix, New, Economics, QuickSavings, CurrencyMix, ExchangeRate,
             RawMaterial, Scrap, IndustrialPerformance, ProtoTooling, Other, Check, Comments,
             idCarga, idCargaSTGBW, idHoja, checksum, iszero)
            VALUES
            (@IdAPICarga, @GuidCarga, @FechaUltModif, @IdCompany, @Ejercicio, @IdCiclo, @IdFase, @IdCurrency, @IdEpigrafe,
             @Volume, @InventoryChange, @Mix, @New, @Economics, @QuickSavings, @CurrencyMix, @ExchangeRate,
             @RawMaterial, @Scrap, @IndustrialPerformance, @ProtoTooling, @Other, @Check, @Comments,
             @IdCarga, @IdCargaSTGBW, @IdHoja, @Checksum, @IsZero)";

        public string GetByIdQuery() => "SELECT * FROM DATA_BridgesMonth WHERE id = @Id";

        public string GetAllQuery() => "SELECT * FROM DATA_BridgesMonth";

        public string UpdateQuery() => @"
            UPDATE DATA_BridgesMonth SET
                IdAPICarga=@IdAPICarga, guidCarga=@GuidCarga, FechaUltModif=@FechaUltModif,
                IdCompany=@IdCompany, Ejercicio=@Ejercicio, IdCiclo=@IdCiclo, IdFase=@IdFase,
                IdCurrency=@IdCurrency, IdEpigrafe=@IdEpigrafe,
                Volume=@Volume, InventoryChange=@InventoryChange, Mix=@Mix, New=@New, Economics=@Economics,
                QuickSavings=@QuickSavings, CurrencyMix=@CurrencyMix, ExchangeRate=@ExchangeRate,
                RawMaterial=@RawMaterial, Scrap=@Scrap, IndustrialPerformance=@IndustrialPerformance,
                ProtoTooling=@ProtoTooling, Other=@Other, Check=@Check, Comments=@Comments,
                idCarga=@IdCarga, idCargaSTGBW=@IdCargaSTGBW, idHoja=@IdHoja,
                checksum=@Checksum, iszero=@IsZero
            WHERE id = @Id";

        public string DeleteQuery() => "DELETE FROM DATA_BridgesMonth WHERE id = @Id";

        public string GetByPlantaEjercicioQuery() => @"
            SELECT * FROM DATA_BridgesMonth
            WHERE IdCompany = @planta AND Ejercicio = @ejercicio ORDER BY IdEpigrafe";
    }
}
