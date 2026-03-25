using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.MySQL
{
    public sealed class MySqlDataTipoCambioQueries : IDataTipoCambioQueries
    {
        public string AddQuery() => @"
            INSERT INTO DATA_Tipo_Cambio
            (idAPICarga, guidCarga, FechaUltModif, ejercicio, idCurrency,
             CalendarDay, mes, P, FC, FB, idCarga, idCargaSTGBW, idHoja, checksum, iszero)
            VALUES
            (@IdAPICarga, @GuidCarga, @FechaUltModif, @Ejercicio, @IdCurrency,
             @CalendarDay, @Mes, @P, @FC, @FB, @IdCarga, @IdCargaSTGBW, @IdHoja, @Checksum, @IsZero)";

        public string GetByIdQuery() => "SELECT * FROM DATA_Tipo_Cambio WHERE id = @Id";

        public string GetAllQuery() => "SELECT * FROM DATA_Tipo_Cambio";

        public string UpdateQuery() => @"
            UPDATE DATA_Tipo_Cambio SET
                idAPICarga=@IdAPICarga, guidCarga=@GuidCarga, FechaUltModif=@FechaUltModif,
                ejercicio=@Ejercicio, idCurrency=@IdCurrency, CalendarDay=@CalendarDay,
                mes=@Mes, P=@P, FC=@FC, FB=@FB,
                idCarga=@IdCarga, idCargaSTGBW=@IdCargaSTGBW, idHoja=@IdHoja,
                checksum=@Checksum, iszero=@IsZero
            WHERE id = @Id";

        public string DeleteQuery() => "DELETE FROM DATA_Tipo_Cambio WHERE id = @Id";

        public string GetByEjercicioQuery() => @"
            SELECT * FROM DATA_Tipo_Cambio
            WHERE ejercicio = @ejercicio ORDER BY CalendarDay";

        public string GetByEjercicioCurrencyQuery() => @"
            SELECT * FROM DATA_Tipo_Cambio
            WHERE ejercicio = @ejercicio AND idCurrency = @idCurrency ORDER BY CalendarDay";
    }
}
