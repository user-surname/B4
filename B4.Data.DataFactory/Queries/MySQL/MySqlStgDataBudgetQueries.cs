using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.MySQL
{
    public sealed class MySqlStgDataBudgetQueries : IStgDataBudgetQueries
    {
        public string AddQuery() => @"
            INSERT INTO STG_DATA_Budget
            (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio, idCiclo, idFase, idCurrency, idEpigrafe, mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10, mes11, mes12, mes13, checksum, iszero)
            VALUES
            (@IdAPICarga, @GuidCarga, @FechaUltModif, @IdCompany, @Ejercicio, @IdCiclo, @IdFase, @IdCurrency, @IdEpigrafe, @Mes00, @Mes01, @Mes02, @Mes03, @Mes04, @Mes05, @Mes06, @Mes07, @Mes08, @Mes09, @Mes10, @Mes11, @Mes12, @Mes13, @Checksum, @IsZero)";

        public string GetByIdQuery() => "SELECT * FROM STG_DATA_Budget WHERE id = @Id";

        public string GetAllQuery() => "SELECT * FROM STG_DATA_Budget";

        public string UpdateQuery() => @"
            UPDATE STG_DATA_Budget SET
                idAPICarga=@IdAPICarga, guidCarga=@GuidCarga, FechaUltModif=@FechaUltModif, idCompany=@IdCompany, ejercicio=@Ejercicio, idCiclo=@IdCiclo, idFase=@IdFase, idCurrency=@IdCurrency, idEpigrafe=@IdEpigrafe, mes00=@Mes00, mes01=@Mes01, mes02=@Mes02, mes03=@Mes03, mes04=@Mes04, mes05=@Mes05, mes06=@Mes06, mes07=@Mes07, mes08=@Mes08, mes09=@Mes09, mes10=@Mes10, mes11=@Mes11, mes12=@Mes12, mes13=@Mes13, checksum=@Checksum, iszero=@IsZero
            WHERE id = @Id";

        public string DeleteQuery() => "DELETE FROM STG_DATA_Budget WHERE id = @Id";

        public string GetByPlantaEjercicioQuery() => @"
            SELECT * FROM STG_DATA_Budget
            WHERE idCompany = @planta AND ejercicio = @ejercicio
            ORDER BY idEpigrafe";
    }
}
