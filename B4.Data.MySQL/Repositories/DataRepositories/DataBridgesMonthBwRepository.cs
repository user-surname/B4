using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces;

namespace B4.Data.MySQL.Repositories.DataRepositories
{
    public class DataBridgesMonthBwRepository : DataRepository<DataBridgesMonthBw>, IDataBridgesMonthBwRepository
    {
        private const string _tableName = "DATA_BridgesMonth_BW";

        private readonly MySQLDapperContext _context;

        public DataBridgesMonthBwRepository(MySQLDapperContext context) : base(context, _tableName)
        {
            _context = context;
        }

        // -------------------------------------------------
        // C - CREATE
        // -------------------------------------------------
        public async Task AddAsync(DataBridgesMonthBw entity)
        {
            const string sql = $@"
                INSERT INTO {_tableName} 
                (idAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                 Volume, InventoryChange, Mix, `New`, Economics, QuickSavings, CurrencyMix, ExchangeRate,
                 RawMaterial, Scrap, IndustrialPerformance, ProtoTooling, Other, `Check`, Comments,
                 idCarga, idCargaSTGBW, idHoja, checksum, iszero)
                VALUES
                (@idAPICarga, @guidCarga, @FechaUltModif, @IdCompany, @Ejercicio, @IdCiclo, @IdFase, @IdCurrency, @IdEpigrafe,
                 @Volume, @InventoryChange, @Mix, @New, @Economics, @QuickSavings, @CurrencyMix, @ExchangeRate,
                 @RawMaterial, @Scrap, @IndustrialPerformance, @ProtoTooling, @Other, @Check, @Comments,
                 @idCarga, @idCargaSTGBW, @idHoja, @checksum, @iszero)";

            try
            {
                using var conn = _context.CreateConnection();
                await conn.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar un registro en DATA_BridgesMonth_BW.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (por ID)
        // -------------------------------------------------
        //public async Task<DataBridgesMonthBw?> GetByIdAsync(int id)
        //{
        //    var sql = $@"SELECT * FROM {_tableName} WHERE Id = @Id";

        //    try
        //    {
        //        using var conn = _context.CreateConnection();
        //        return await conn.QuerySingleOrDefaultAsync<DataBridgesMonthBw>(sql, new { Id = id });
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Error al obtener el registro con Id {id}.", ex);
        //    }
        //}

        //// -------------------------------------------------
        //// R - READ (todos)
        //// -------------------------------------------------
        //public async Task<IEnumerable<DataBridgesMonthBw>> GetAllAsync()
        //{
        //    var sql = $@"SELECT * FROM {_tableName}";

        //    try
        //    {
        //        using var conn = _context.CreateConnection();
        //        return await conn.QueryAsync<DataBridgesMonthBw>(sql);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error al obtener todos los registros de DATA_BridgesMonth_BW.", ex);
        //    }
        //}

        // -------------------------------------------------
        // U - UPDATE
        // -------------------------------------------------
        public async Task UpdateAsync(DataBridgesMonthBw entity)
        {
            const string sql = $@"
                UPDATE {_tableName} SET
                    idAPICarga = @idAPICarga,
                    guidCarga = @guidCarga,
                    FechaUltModif = @FechaUltModif,
                    IdCompany = @IdCompany,
                    Ejercicio = @Ejercicio,
                    IdCiclo = @IdCiclo,
                    IdFase = @IdFase,
                    IdCurrency = @IdCurrency,
                    IdEpigrafe = @IdEpigrafe,
                    Volume = @Volume,
                    InventoryChange = @InventoryChange,
                    Mix = @Mix,
                    `New` = @New,
                    Economics = @Economics,
                    QuickSavings = @QuickSavings,
                    CurrencyMix = @CurrencyMix,
                    ExchangeRate = @ExchangeRate,
                    RawMaterial = @RawMaterial,
                    Scrap = @Scrap,
                    IndustrialPerformance = @IndustrialPerformance,
                    ProtoTooling = @ProtoTooling,
                    Other = @Other,
                    `Check` = @Check,
                    Comments = @Comments,
                    idCarga = @idCarga,
                    idCargaSTGBW = @idCargaSTGBW,
                    idHoja = @idHoja,
                    checksum = @checksum,
                    iszero = @iszero
                WHERE Id = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                var rows = await conn.ExecuteAsync(sql, entity);
                if (rows == 0)
                    throw new Exception($"No se encontró el registro con Id {entity.Id} para actualizar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el registro con Id {entity.Id}.", ex);
            }
        }

        // -------------------------------------------------
        // D - DELETE
        // -------------------------------------------------
        //public async Task DeleteAsync(int id)
        //{
        //    var sql = $@"DELETE FROM {_tableName} WHERE Id = @Id";

        //    try
        //    {
        //        using var conn = _context.CreateConnection();
        //        var rows = await conn.ExecuteAsync(sql, new { Id = id });
        //        if (rows == 0)
        //            throw new Exception($"No se encontró el registro con Id {id} para eliminar.");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Error al eliminar el registro con Id {id}.", ex);
        //    }
        //}
    }
}

