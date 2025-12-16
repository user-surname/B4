using Dapper;
using B4.Models.Entities.DataEntities;
using B4.Models.Interfaces.DataInterfaces;

namespace B4.Data.PostgreSQL.Repositories.DataRepositories
{
    public class DataTipoCambioRepository 
        : BaseRepository<DataTipoCambio>, IDataTipoCambioRepository
    {
        public DataTipoCambioRepository(PostgreSQLDapperContext context)
            : base(context, "b4.data_tipo_cambio") { }

        public async Task AddAsync(DataTipoCambio entity)
        {
            var sql = @"
                INSERT INTO b4.data_tipo_cambio
                (idapicarga, guidcarga, fechaultmodif,
                 ejercicio, idcurrency, calendarday, mes,
                 p, fc, fb, idcarga, idcargastgbw, idhoja)
                VALUES
                (@IdAPICarga, @GuidCarga, @FechaUltModif,
                 @Ejercicio, @IdCurrency, @CalendarDay, @Mes,
                 @P, @FC, @FB, @IdCarga, @IdCargaSTGBW, @IdHoja)
                RETURNING id;";

            using var conn = GetConnection();
            entity.Id = await conn.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task UpdateAsync(DataTipoCambio entity)
        {
            var sql = @"
                UPDATE b4.data_tipo_cambio SET
                    idapicarga=@IdAPICarga,
                    guidcarga=@GuidCarga,
                    fechaultmodif=@FechaUltModif,
                    ejercicio=@Ejercicio,
                    idcurrency=@IdCurrency,
                    calendarday=@CalendarDay,
                    mes=@Mes,
                    p=@P, fc=@FC, fb=@FB,
                    idcarga=@IdCarga, idcargastgbw=@IdCargaSTGBW, idhoja=@IdHoja
                WHERE id=@Id";

            using var conn = GetConnection();
            await conn.ExecuteAsync(sql, entity);
        }
    }
}
