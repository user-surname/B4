using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.MySQL
{
    /// <summary>
    /// Implementa la version MySQL de las queries de DataActualsBw.
    /// DataQueryProvider devuelve esta clase cuando "bbdd" vale "MySQL".
    /// </summary>
    public sealed class MySqlDataActualsBwQueries : IDataActualsBwQueries
    {
        /// <summary>
        /// Obtiene la query para insertar un DataActualsBw en MySQL.
        /// </summary>
        public string AddQuery()
        {
            return @"
                INSERT INTO DATA_Actuals_BW
                (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio, idCiclo, idFase, idCurrency, idEpigrafe,
                 mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10, mes11, mes12, mes13, checksum, iszero)
                VALUES
                (@IdAPICarga, @GuidCarga, @FechaUltModif, @IdCompany, @Ejercicio, @IdCiclo, @IdFase, @IdCurrency, @IdEpigrafe,
                 @Mes00, @Mes01, @Mes02, @Mes03, @Mes04, @Mes05, @Mes06, @Mes07, @Mes08, @Mes09, @Mes10, @Mes11, @Mes12, @Mes13, @Checksum, @IsZero)";
        }

        /// <summary>
        /// Obtiene la query para recuperar un DataActualsBw por identificador en MySQL.
        /// </summary>
        public string GetByIdQuery()
        {
            return "SELECT * FROM DATA_Actuals_BW WHERE id = @Id";
        }

        /// <summary>
        /// Obtiene la query para recuperar todos los registros de DataActualsBw en MySQL.
        /// </summary>
        public string GetAllQuery()
        {
            return "SELECT * FROM DATA_Actuals_BW";
        }

        /// <summary>
        /// Obtiene la query para actualizar un DataActualsBw en MySQL.
        /// </summary>
        public string UpdateQuery()
        {
            return @"
                UPDATE DATA_Actuals_BW SET
                    idAPICarga = @IdAPICarga,
                    guidCarga = @GuidCarga,
                    FechaUltModif = @FechaUltModif,
                    idCompany = @IdCompany,
                    ejercicio = @Ejercicio,
                    idCiclo = @IdCiclo,
                    idFase = @IdFase,
                    idCurrency = @IdCurrency,
                    idEpigrafe = @IdEpigrafe,
                    mes00 = @Mes00, mes01 = @Mes01, mes02 = @Mes02, mes03 = @Mes03, mes04 = @Mes04,
                    mes05 = @Mes05, mes06 = @Mes06, mes07 = @Mes07, mes08 = @Mes08, mes09 = @Mes09,
                    mes10 = @Mes10, mes11 = @Mes11, mes12 = @Mes12, mes13 = @Mes13,
                    checksum = @Checksum,
                    iszero = @IsZero
                WHERE id = @Id";
        }

        /// <summary>
        /// Obtiene la query para eliminar un DataActualsBw en MySQL.
        /// </summary>
        public string DeleteQuery()
        {
            return "DELETE FROM DATA_Actuals_BW WHERE id = @Id";
        }
    }
}
