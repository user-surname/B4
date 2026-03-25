using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.PostgreSQL
{
    public sealed class PostgreSqlDataBridgesFyQueries : IDataBridgesFyQueries
    {
        public string AddQuery() => @"
            INSERT INTO b4.data_bridges_fy
            (idapicarga, guidcarga, fechaultmodif, idcompany, ejercicio, idciclo, idfase, idcurrency, idepigrafe,
             fiscalyear, percentage, zero, zeropercentage, absolute, absolutepercentage,
             vmixnew, rawmaterial, scrap, economics, currencymix, performance, prototool, others, comments)
            VALUES
            (@IdAPICarga, @GuidCarga, @FechaUltModif, @IdCompany, @Ejercicio, @IdCiclo, @IdFase, @IdCurrency, @IdEpigrafe,
             @FiscalYear, @Percentage, @Zero, @ZeroPercentage, @Absolute, @AbsolutePercentage,
             @VMixNew, @RawMaterial, @Scrap, @Economics, @CurrencyMix, @Performance, @ProtoTool, @Others, @Comments)
            RETURNING id";

        public string GetByIdQuery() => "SELECT * FROM b4.data_bridges_fy WHERE id = @Id";

        public string GetAllQuery() => "SELECT * FROM b4.data_bridges_fy LIMIT 2000";

        public string UpdateQuery() => @"
            UPDATE b4.data_bridges_fy SET
                idapicarga=@IdAPICarga, guidcarga=@GuidCarga, fechaultmodif=@FechaUltModif,
                idcompany=@IdCompany, ejercicio=@Ejercicio, idciclo=@IdCiclo, idfase=@IdFase,
                idcurrency=@IdCurrency, idepigrafe=@IdEpigrafe,
                fiscalyear=@FiscalYear, percentage=@Percentage, zero=@Zero, zeropercentage=@ZeroPercentage,
                absolute=@Absolute, absolutepercentage=@AbsolutePercentage, vmixnew=@VMixNew,
                rawmaterial=@RawMaterial, scrap=@Scrap, economics=@Economics, currencymix=@CurrencyMix,
                performance=@Performance, prototool=@ProtoTool, others=@Others, comments=@Comments
            WHERE id=@Id";

        public string DeleteQuery() => "DELETE FROM b4.data_bridges_fy WHERE id = @Id";

        public string GetByPlantaEjercicioQuery() => @"
            SELECT * FROM b4.data_bridges_fy
            WHERE idcompany = @planta AND ejercicio = @ejercicio ORDER BY idepigrafe";
    }
}
