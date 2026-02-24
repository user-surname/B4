using Dapper;
using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces;

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
                    p=@P,
                    fc=@FC,
                    fb=@FB,
                    idcarga=@IdCarga,
                    idcargastgbw=@IdCargaSTGBW,
                    idhoja=@IdHoja
                WHERE id=@Id;";

            using var conn = GetConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        // ✅ AÑADIDO: GetByIdAsync seguro (por si tus tests usan BaseRepository y hace SELECT *)
        public async Task<DataTipoCambio?> GetByIdAsync(int id)
        {
            var sql = @"
                SELECT
                    id                              AS ""Id"",
                    idapicarga                      AS ""IdAPICarga"",
                    guidcarga                       AS ""GuidCarga"",
                    fechaultmodif                   AS ""FechaUltModif"",
                    ejercicio                       AS ""Ejercicio"",
                    idcurrency                      AS ""IdCurrency"",
                    calendarday::timestamp          AS ""CalendarDay"",
                    mes                             AS ""Mes"",
                    p                               AS ""P"",
                    fc                              AS ""FC"",
                    fb                              AS ""FB"",
                    idcarga                         AS ""IdCarga"",
                    idcargastgbw                    AS ""IdCargaSTGBW"",
                    idhoja                          AS ""IdHoja"",
                    createdat                       AS ""CreatedAt"",
                    updatedat                       AS ""UpdatedAt"",
                    version                         AS ""Version"",
                    checksum                        AS ""Checksum"",
                    iszero                          AS ""IsZero""
                FROM b4.data_tipo_cambio
                WHERE id = @id;";

            using var conn = GetConnection();
            return await conn.QuerySingleOrDefaultAsync<DataTipoCambio>(sql, new { id });
        }

        public async Task<IEnumerable<DataTipoCambio>> GetByEjercicioAsync(int ejercicio)
        {
            var sql = @"
                SELECT
                    id                              AS ""Id"",
                    idapicarga                      AS ""IdAPICarga"",
                    guidcarga                       AS ""GuidCarga"",
                    fechaultmodif                   AS ""FechaUltModif"",
                    ejercicio                       AS ""Ejercicio"",
                    idcurrency                      AS ""IdCurrency"",
                    calendarday::timestamp          AS ""CalendarDay"",
                    mes                             AS ""Mes"",
                    p                               AS ""P"",
                    fc                              AS ""FC"",
                    fb                              AS ""FB"",
                    idcarga                         AS ""IdCarga"",
                    idcargastgbw                    AS ""IdCargaSTGBW"",
                    idhoja                          AS ""IdHoja"",
                    createdat                       AS ""CreatedAt"",
                    updatedat                       AS ""UpdatedAt"",
                    version                         AS ""Version"",
                    checksum                        AS ""Checksum"",
                    iszero                          AS ""IsZero""
                FROM b4.data_tipo_cambio
                WHERE ejercicio = @ejercicio
                ORDER BY calendarday;";

            using var conn = GetConnection();
            return await conn.QueryAsync<DataTipoCambio>(sql, new { ejercicio });
        }

        public async Task<IEnumerable<DataTipoCambio>> GetByEjercicioCurrencyAsync(int ejercicio, int idCurrency)
        {
            var sql = @"
                SELECT
                    id                              AS ""Id"",
                    idapicarga                      AS ""IdAPICarga"",
                    guidcarga                       AS ""GuidCarga"",
                    fechaultmodif                   AS ""FechaUltModif"",
                    ejercicio                       AS ""Ejercicio"",
                    idcurrency                      AS ""IdCurrency"",
                    calendarday::timestamp          AS ""CalendarDay"",
                    mes                             AS ""Mes"",
                    p                               AS ""P"",
                    fc                              AS ""FC"",
                    fb                              AS ""FB"",
                    idcarga                         AS ""IdCarga"",
                    idcargastgbw                    AS ""IdCargaSTGBW"",
                    idhoja                          AS ""IdHoja"",
                    createdat                       AS ""CreatedAt"",
                    updatedat                       AS ""UpdatedAt"",
                    version                         AS ""Version"",
                    checksum                        AS ""Checksum"",
                    iszero                          AS ""IsZero""
                FROM b4.data_tipo_cambio
                WHERE ejercicio = @ejercicio
                  AND idcurrency = @idCurrency
                ORDER BY calendarday;";

            using var conn = GetConnection();
            return await conn.QueryAsync<DataTipoCambio>(sql, new { ejercicio, idCurrency });
        }
    }
}