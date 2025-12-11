using Dapper;
using Npgsql;
using B4.Models.Entities;


namespace B4.Data.PostgreSQL.Repositories
{
    public abstract class BaseFinancialRepository<T> : BaseRepository<T> 
        where T : DataBaseFinanciero
    {
        protected BaseFinancialRepository(string connectionString, string table)
            : base(connectionString, table) { }

        // CREATE para entidades financieras
        public override async Task AddAsync(T entity)
        {
            var sql = $@"
                INSERT INTO {_table}
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

            using var conn = GetConnection();
            entity.Id = await conn.ExecuteScalarAsync<int>(sql, entity);
        }

        // UPDATE para entidades financieras
        public override async Task UpdateAsync(T entity)
        {
            var sql = $@"
                UPDATE {_table} SET
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
                    idcarga=@IdCarga, idcargastgbw=@IdCargaSTGBW, idhoja=@IdHoja
                WHERE id=@Id";

            using var conn = GetConnection();
            await conn.ExecuteAsync(sql, entity);
        }
    }
}
