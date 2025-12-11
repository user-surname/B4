using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Data.MySQL.Repositories.DataRepositories
{
    public abstract class DataRepository<T> where T : class
    {
        protected readonly MySQLDapperContext _context;
        protected readonly string _tableName;

        protected DataRepository(MySQLDapperContext context, string tableName)
        {
            _context = context;
            _tableName = tableName;
        }

        // -------------------------
        // CREATE
        // -------------------------
        //public async Task AddAsync(T entity)
        //{
        //    var sql = $@"
        //        INSERT INTO {_tableName} 
        //        (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio, idCiclo, idFase, idCurrency, idEpigrafe,
        //         mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10, mes11, mes12, mes13)
        //        VALUES
        //        (@idAPICarga, @guidCarga, @FechaUltModif, @idCompany, @ejercicio, @idCiclo, @idFase, @idCurrency, @idEpigrafe,
        //         @mes00, @mes01, @mes02, @mes03, @mes04, @mes05, @mes06, @mes07, @mes08, @mes09, @mes10, @mes11, @mes12, @mes13)";

        //    try
        //    {
        //        using var conn = _context.CreateConnection();
        //        await conn.ExecuteAsync(sql, entity);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error al insertar un registro en " + _tableName, ex);
        //    }
        //}

        // -------------------------
        // READ BY ID
        // -------------------------
        public async Task<T?> GetByIdAsync(int id)
        {


            var sql = $@"SELECT * FROM {_tableName} WHERE id = @Id";
            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<T>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el registro con Id {id} de " + _tableName, ex);
            }

        }

        // -------------------------
        // READ ALL
        // -------------------------
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var sql = $@"SELECT * FROM {_tableName}";
            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QueryAsync<T>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener todos los registros de " + _tableName, ex);
            }
        }

        // -------------------------
        // UPDATE
        // -------------------------
        //public async Task UpdateAsync(T entity)
        //{
        //    var sql = $@"
        //        UPDATE {_tableName} SET
        //            idAPICarga = @IdAPICarga,
        //            guidCarga = @GuidCarga,
        //            FechaUltModif = @FechaUltModif,
        //            idCompany = @IdCompany,
        //            ejercicio = @Ejercicio,
        //            idCiclo = @IdCiclo,
        //            idFase = @IdFase,
        //            idCurrency = @IdCurrency,
        //            idEpigrafe = @IdEpigrafe,
        //            mes00 = @Mes00, mes01 = @Mes01, mes02 = @Mes02, mes03 = @Mes03, mes04 = @Mes04,
        //            mes05 = @Mes05, mes06 = @Mes06, mes07 = @Mes07, mes08 = @Mes08, mes09 = @Mes09,
        //            mes10 = @Mes10, mes11 = @Mes11, mes12 = @Mes12, mes13 = @Mes13
        //        WHERE id = @Id";

        //    try
        //    {
        //        using var conn = _context.CreateConnection();
        //        var rows = await conn.ExecuteAsync(sql, entity);
        //        if (rows == 0)
        //            throw new Exception($"No se encontró el registro con Id {entity.Id} para actualizar.");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Error al actualizar el registro con Id {entity.Id}.", ex);
        //    }
        //}

        // -------------------------
        // DELETE
        // -------------------------
        public async Task DeleteAsync(int id)
        {
            var sql = $@"DELETE FROM {_tableName} WHERE id = @Id";
            try
            {
                using var conn = _context.CreateConnection();
                var rows = await conn.ExecuteAsync(sql, new { Id = id });
                if (rows == 0)
                    throw new Exception($"No se encontró el registro con Id {id} para eliminar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el registro con Id {id}.", ex);
            }
        }
    }
}
