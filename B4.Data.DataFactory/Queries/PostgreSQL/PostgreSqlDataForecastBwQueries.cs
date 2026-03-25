using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.PostgreSQL
{
    public sealed class PostgreSqlDataForecastBwQueries : IDataForecastBwQueries
    {
        public string AddQuery() => @"
            INSERT INTO b4.data_forecast_bw
            (idapicarga, guidcarga, fechaultmodif, idcompany, ejercicio, idciclo, idfase, idcurrency, idepigrafe, mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10, mes11, mes12, mes13, idcarga, idcargastgbw, idhoja)
            VALUES
            (@IdAPICarga, @GuidCarga, @FechaUltModif, @IdCompany, @Ejercicio, @IdCiclo, @IdFase, @IdCurrency, @IdEpigrafe, @Mes00, @Mes01, @Mes02, @Mes03, @Mes04, @Mes05, @Mes06, @Mes07, @Mes08, @Mes09, @Mes10, @Mes11, @Mes12, @Mes13, @IdCarga, @IdCargaSTGBW, @IdHoja)
            RETURNING id";

        public string GetByIdQuery() => "SELECT * FROM b4.data_forecast_bw WHERE id = @Id";

        public string GetAllQuery() => "SELECT * FROM b4.data_forecast_bw LIMIT 2000";

        public string UpdateQuery() => @"
            UPDATE b4.data_forecast_bw SET
                idapicarga=@IdAPICarga, guidcarga=@GuidCarga, fechaultmodif=@FechaUltModif, idcompany=@IdCompany, ejercicio=@Ejercicio, idciclo=@IdCiclo, idfase=@IdFase, idcurrency=@IdCurrency, idepigrafe=@IdEpigrafe, mes00=@Mes00, mes01=@Mes01, mes02=@Mes02, mes03=@Mes03, mes04=@Mes04, mes05=@Mes05, mes06=@Mes06, mes07=@Mes07, mes08=@Mes08, mes09=@Mes09, mes10=@Mes10, mes11=@Mes11, mes12=@Mes12, mes13=@Mes13, idcarga=@IdCarga, idcargastgbw=@IdCargaSTGBW, idhoja=@IdHoja
            WHERE id=@Id";

        public string DeleteQuery() => "DELETE FROM b4.data_forecast_bw WHERE id = @Id";

        public string GetByPlantaEjercicioQuery() => @"
            SELECT * FROM b4.data_forecast_bw
            WHERE idcompany = @planta AND ejercicio = @ejercicio ORDER BY idepigrafe";
    }
}
