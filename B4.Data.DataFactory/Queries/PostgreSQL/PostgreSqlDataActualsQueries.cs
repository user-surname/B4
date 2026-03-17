using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.PostgreSQL
{
    /// <summary>
    /// Implementa la version PostgreSQL de las queries de DataActuals.
    /// DataQueryProvider devuelve esta clase cuando "bbdd" vale "PostgreSQL".
    /// </summary>
    public sealed class PostgreSqlDataActualsQueries : IDataActualsQueries
    {
        /// <summary>
        /// Obtiene la query para insertar un DataActuals en PostgreSQL.
        /// </summary>
        public string AddQuery()
        {
            return @"
                INSERT INTO b4.data_actuals
                (idapicarga, guidcarga, fechaultmodif,
                 idcompany, ejercicio, idciclo, idfase,
                 idcurrency, idepigrafe,
                 mes00, mes01, mes02, mes03, mes04, mes05, mes06,
                 mes07, mes08, mes09, mes10, mes11, mes12, mes13,
                 idcarga, idcargastgbw, idhoja)
                VALUES (
                 @IdAPICarga, @GuidCarga, @FechaUltModif,
                 @IdCompany, @Ejercicio, @IdCiclo, @IdFase,
                 @IdCurrency, @IdEpigrafe,
                 @Mes00, @Mes01, @Mes02, @Mes03, @Mes04, @Mes05, @Mes06,
                 @Mes07, @Mes08, @Mes09, @Mes10, @Mes11, @Mes12, @Mes13,
                 @IdCarga, @IdCargaSTGBW, @IdHoja
                )
                RETURNING id;";
        }

        /// <summary>
        /// Obtiene la query para recuperar un DataActuals por identificador en PostgreSQL.
        /// </summary>
        public string GetByIdQuery()
        {
            return "SELECT * FROM b4.data_actuals WHERE id = @Id";
        }

        /// <summary>
        /// Obtiene la query para recuperar todos los registros de DataActuals en PostgreSQL.
        /// </summary>
        public string GetAllQuery()
        {
            return "SELECT * FROM b4.data_actuals LIMIT 2000";
        }

        /// <summary>
        /// Obtiene la query para actualizar un DataActuals en PostgreSQL.
        /// </summary>
        public string UpdateQuery()
        {
            return @"
                UPDATE b4.data_actuals SET
                    idapicarga=@IdAPICarga,
                    guidcarga=@GuidCarga,
                    fechaultmodif=@FechaUltModif,
                    idcompany=@IdCompany,
                    ejercicio=@Ejercicio,
                    idciclo=@IdCiclo,
                    idfase=@IdFase,
                    idcurrency=@IdCurrency,
                    idepigrafe=@IdEpigrafe,
                    mes00=@Mes00, mes01=@Mes01, mes02=@Mes02, mes03=@Mes03,
                    mes04=@Mes04, mes05=@Mes05, mes06=@Mes06,
                    mes07=@Mes07, mes08=@Mes08, mes09=@Mes09,
                    mes10=@Mes10, mes11=@Mes11, mes12=@Mes12, mes13=@Mes13,
                    idcarga=@IdCarga,
                    idcargastgbw=@IdCargaSTGBW,
                    idhoja=@IdHoja
                WHERE id=@Id";
        }

        /// <summary>
        /// Obtiene la query para eliminar un DataActuals en PostgreSQL.
        /// </summary>
        public string DeleteQuery()
        {
            return "DELETE FROM b4.data_actuals WHERE id = @Id";
        }

        /// <summary>
        /// Obtiene la query para recuperar DataActuals por planta y ejercicio en PostgreSQL.
        /// </summary>
        public string GetByPlantaEjercicioQuery()
        {
            return @"
                SELECT *
                FROM b4.data_actuals
                WHERE idcompany = @planta
                  AND ejercicio = @ejercicio
                ORDER BY idepigrafe;";
        }

        /// <summary>
        /// Obtiene la query para recuperar DataActuals por planta, ejercicio y epigrafe en PostgreSQL.
        /// </summary>
        public string GetByPlantaEjercicioEpigrafeQuery()
        {
            return @"
                SELECT *
                FROM b4.data_actuals
                WHERE idcompany = @planta
                  AND ejercicio = @ejercicio
                  AND idepigrafe = @epigrafe
                LIMIT 1;";
        }
    }
}
