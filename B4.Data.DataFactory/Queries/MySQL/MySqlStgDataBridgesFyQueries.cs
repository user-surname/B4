using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.MySQL
{
    public sealed class MySqlStgDataBridgesFyQueries : IStgDataBridgesFyQueries
    {
        public string AddQuery() => @"
            INSERT INTO STG_DATA_BridgesFY
            (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
             FiscalYear, Percentage, Zero, ZeroPercentage, Absolute, AbsolutePercentage,
             VMixNew, RawMaterial, Scrap, Economics, CurrencyMix, Performance, ProtoTool, Others, Comments, checksum, iszero)
            VALUES
            (@IdAPICarga, @GuidCarga, @FechaUltModif, @IdCompany, @Ejercicio, @IdCiclo, @IdFase, @IdCurrency, @IdEpigrafe,
             @FiscalYear, @Percentage, @Zero, @ZeroPercentage, @Absolute, @AbsolutePercentage,
             @VMixNew, @RawMaterial, @Scrap, @Economics, @CurrencyMix, @Performance, @ProtoTool, @Others, @Comments, @Checksum, @IsZero)";

        public string GetByIdQuery() => "SELECT * FROM STG_DATA_BridgesFY WHERE id = @Id";

        public string GetAllQuery() => "SELECT * FROM STG_DATA_BridgesFY";

        public string UpdateQuery() => @"
            UPDATE STG_DATA_BridgesFY SET
                IdAPICarga=@IdAPICarga, guidCarga=@GuidCarga, FechaUltModif=@FechaUltModif,
                IdCompany=@IdCompany, Ejercicio=@Ejercicio, IdCiclo=@IdCiclo, IdFase=@IdFase,
                IdCurrency=@IdCurrency, IdEpigrafe=@IdEpigrafe,
                FiscalYear=@FiscalYear, Percentage=@Percentage, Zero=@Zero, ZeroPercentage=@ZeroPercentage,
                Absolute=@Absolute, AbsolutePercentage=@AbsolutePercentage,
                VMixNew=@VMixNew, RawMaterial=@RawMaterial, Scrap=@Scrap, Economics=@Economics,
                CurrencyMix=@CurrencyMix, Performance=@Performance, ProtoTool=@ProtoTool,
                Others=@Others, Comments=@Comments, checksum=@Checksum, iszero=@IsZero
            WHERE id = @Id";

        public string DeleteQuery() => "DELETE FROM STG_DATA_BridgesFY WHERE id = @Id";

        public string GetByPlantaEjercicioQuery() => @"
            SELECT * FROM STG_DATA_BridgesFY
            WHERE IdCompany = @planta AND Ejercicio = @ejercicio ORDER BY IdEpigrafe";
    }
}
