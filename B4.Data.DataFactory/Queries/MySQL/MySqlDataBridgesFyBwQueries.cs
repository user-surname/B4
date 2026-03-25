using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.MySQL
{
    public sealed class MySqlDataBridgesFyBwQueries : IDataBridgesFyBwQueries
    {
        public string AddQuery() => @"
            INSERT INTO DATA_BridgesFY_BW
            (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
             FiscalYear, Percentage, Zero, ZeroPercentage, Absolute, AbsolutePercentage,
             VMixNew, RawMaterial, Scrap, Economics, CurrencyMix, Performance, ProtoTool, Others, Comments,
             idCarga, idCargaSTGBW, idHoja, checksum, iszero)
            VALUES
            (@IdAPICarga, @GuidCarga, @FechaUltModif, @IdCompany, @Ejercicio, @IdCiclo, @IdFase, @IdCurrency, @IdEpigrafe,
             @FiscalYear, @Percentage, @Zero, @ZeroPercentage, @Absolute, @AbsolutePercentage,
             @VMixNew, @RawMaterial, @Scrap, @Economics, @CurrencyMix, @Performance, @ProtoTool, @Others, @Comments,
             @IdCarga, @IdCargaSTGBW, @IdHoja, @Checksum, @IsZero)";

        public string GetByIdQuery() => "SELECT * FROM DATA_BridgesFY_BW WHERE id = @Id";

        public string GetAllQuery() => "SELECT * FROM DATA_BridgesFY_BW";

        public string UpdateQuery() => @"
            UPDATE DATA_BridgesFY_BW SET
                IdAPICarga=@IdAPICarga, guidCarga=@GuidCarga, FechaUltModif=@FechaUltModif,
                IdCompany=@IdCompany, Ejercicio=@Ejercicio, IdCiclo=@IdCiclo, IdFase=@IdFase,
                IdCurrency=@IdCurrency, IdEpigrafe=@IdEpigrafe,
                FiscalYear=@FiscalYear, Percentage=@Percentage, Zero=@Zero, ZeroPercentage=@ZeroPercentage,
                Absolute=@Absolute, AbsolutePercentage=@AbsolutePercentage,
                VMixNew=@VMixNew, RawMaterial=@RawMaterial, Scrap=@Scrap, Economics=@Economics,
                CurrencyMix=@CurrencyMix, Performance=@Performance, ProtoTool=@ProtoTool,
                Others=@Others, Comments=@Comments,
                idCarga=@IdCarga, idCargaSTGBW=@IdCargaSTGBW, idHoja=@IdHoja,
                checksum=@Checksum, iszero=@IsZero
            WHERE id = @Id";

        public string DeleteQuery() => "DELETE FROM DATA_BridgesFY_BW WHERE id = @Id";

        public string GetByPlantaEjercicioQuery() => @"
            SELECT * FROM DATA_BridgesFY_BW
            WHERE IdCompany = @planta AND Ejercicio = @ejercicio ORDER BY IdEpigrafe";
    }
}
