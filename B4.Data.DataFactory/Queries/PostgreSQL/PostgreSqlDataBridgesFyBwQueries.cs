using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.PostgreSQL
{
    public sealed class PostgreSqlDataBridgesFyBwQueries : IDataBridgesFyBwQueries
    {
        public string AddQuery() => @"
            INSERT INTO b4.data_bridges_fy_bw
            (idapicarga, guidcarga, fechaultmodif, idcompany, ejercicio, idciclo, idfase, idcurrency, idepigrafe,
             fiscalyear, percentage, zero, zeropercentage, absolute, absolutepercentage,
             vmixnew, rawmaterial, scrap, economics, currencymix, performance, prototool, others, comments,
             idcarga, idcargastgbw, idhoja)
            VALUES
            (@IdAPICarga, @GuidCarga, @FechaUltModif, @IdCompany, @Ejercicio, @IdCiclo, @IdFase, @IdCurrency, @IdEpigrafe,
             @FiscalYear, @Percentage, @Zero, @ZeroPercentage, @Absolute, @AbsolutePercentage,
             @VMixNew, @RawMaterial, @Scrap, @Economics, @CurrencyMix, @Performance, @ProtoTool, @Others, @Comments,
             @IdCarga, @IdCargaSTGBW, @IdHoja)
            RETURNING id";

        public string GetByIdQuery() => "SELECT * FROM b4.data_bridges_fy_bw WHERE id = @Id";

        public string GetAllQuery() => "SELECT * FROM b4.data_bridges_fy_bw LIMIT 2000";

        public string UpdateQuery() => @"
            UPDATE b4.data_bridges_fy_bw SET
                idapicarga=@IdAPICarga, guidcarga=@GuidCarga, fechaultmodif=@FechaUltModif,
                idcompany=@IdCompany, ejercicio=@Ejercicio, idciclo=@IdCiclo, idfase=@IdFase,
                idcurrency=@IdCurrency, idepigrafe=@IdEpigrafe,
                fiscalyear=@FiscalYear, percentage=@Percentage, zero=@Zero, zeropercentage=@ZeroPercentage,
                absolute=@Absolute, absolutepercentage=@AbsolutePercentage, vmixnew=@VMixNew,
                rawmaterial=@RawMaterial, scrap=@Scrap, economics=@Economics, currencymix=@CurrencyMix,
                performance=@Performance, prototool=@ProtoTool, others=@Others, comments=@Comments,
                idcarga=@IdCarga, idcargastgbw=@IdCargaSTGBW, idhoja=@IdHoja
            WHERE id=@Id";

        public string DeleteQuery() => "DELETE FROM b4.data_bridges_fy_bw WHERE id = @Id";

        public string GetByPlantaEjercicioQuery() => @"
            SELECT * FROM b4.data_bridges_fy_bw
            WHERE idcompany = @planta AND ejercicio = @ejercicio ORDER BY idepigrafe";
    }
}
