using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.PostgreSQL
{
    public sealed class PostgreSqlDataTipoCambioQueries : IDataTipoCambioQueries
    {
        public string AddQuery() => @"
            INSERT INTO b4.data_tipo_cambio
            (idapicarga, guidcarga, fechaultmodif, ejercicio, idcurrency, calendarday, mes, p, fc, fb, idcarga, idcargastgbw, idhoja)
            VALUES
            (@IdAPICarga, @GuidCarga, @FechaUltModif, @Ejercicio, @IdCurrency, @CalendarDay, @Mes, @P, @FC, @FB, @IdCarga, @IdCargaSTGBW, @IdHoja)
            RETURNING id";

        public string GetByIdQuery() => "SELECT * FROM b4.data_tipo_cambio WHERE id = @Id";

        public string GetAllQuery() => "SELECT * FROM b4.data_tipo_cambio LIMIT 2000";

        public string UpdateQuery() => @"
            UPDATE b4.data_tipo_cambio SET
                idapicarga=@IdAPICarga, guidcarga=@GuidCarga, fechaultmodif=@FechaUltModif,
                ejercicio=@Ejercicio, idcurrency=@IdCurrency, calendarday=@CalendarDay,
                mes=@Mes, p=@P, fc=@FC, fb=@FB,
                idcarga=@IdCarga, idcargastgbw=@IdCargaSTGBW, idhoja=@IdHoja
            WHERE id=@Id";

        public string DeleteQuery() => "DELETE FROM b4.data_tipo_cambio WHERE id = @Id";

        public string GetByEjercicioQuery() => @"
            SELECT * FROM b4.data_tipo_cambio
            WHERE ejercicio = @ejercicio ORDER BY calendarday";

        public string GetByEjercicioCurrencyQuery() => @"
            SELECT * FROM b4.data_tipo_cambio
            WHERE ejercicio = @ejercicio AND idcurrency = @idCurrency ORDER BY calendarday";
    }
}
