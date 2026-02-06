/*
    Service: MemoryCache

    20251111 - pendiente implementacion especifica para B4
 */

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper;
//using GESTAMP_API.Repository;
//using Microsoft.Data.SqlClient;
//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using static System.Runtime.InteropServices.JavaScript.JSType;
using NLog;
using System.Reflection.Metadata.Ecma335;
using B4.Models.RepositoryInterfaces.LkInterfaces;


namespace B4.Data.Services
{
    /// <summary>
    /// Servicio de cache para tablas maestras
    /// incluye los Interfaz, modelos y la gestion de cache
    /// </summary>

    // TODO: revisar lo que viene de la vista V_COMPANY

    public static class CacheKeys
    {
        public const string LkEjemplo = "Ejemplo";
    }

    public class LkEjemplo
    {
        [Key]
        [Column("IdEjemplo")]
        public int IdEjemplo { get; set; } = 0;
        [Column("Descripcion")]
        public string Descripcion { get; set; } = string.Empty;
    }

    /// <summary>
    /// Cache service proporciona acceso eficiente a las tablas maestras
    /// Los datos se cargan desde bbdd solo si no los tenemos en cache o si han expirado de la cache
    /// Los datos expiran 1 hora despues de haberse cargado o cada 24 horas (ver GetCachedDataAsync)
    /// </summary>
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly ILogger<MemoryCacheService> _logger;

        private readonly IMemoryCache _cache;
        private readonly string _connectionString;

        #region Funciones base
        ///
        /// constructor con IConfiguration
        /// TODO: revisar !!!
        /// 
        //public MemoryCacheService(IMemoryCache cache, IConfiguration configuration, ILogger<MemoryCacheService> logger)
        //{
        //    _logger = logger;
        //    _cache = cache;
        //    _connectionString = configuration.GetConnectionString("SQL");
        //}

        /// <summary>
        /// TODO: revisar cadena de conexexion a BBDD
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="logger"></param>
        public MemoryCacheService(IMemoryCache cache, ILogger<MemoryCacheService> logger)
        {
            _logger = logger;

            _cache = cache;

            //_connectionString = configuration.GetConnectionString("SQL");
        }


        /// <summary>
        /// Funcion principal para hace cache de cahceKey
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="getData"></param>
        /// <returns></returns>
        private async Task<IEnumerable<T>> GetCachedDataAsync<T>(string cacheKey, Func<Task<IEnumerable<T>>> getData)
        {
            _logger.LogDebug($"GetCachedDataAsync: {cacheKey}");

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<T> cachedData))
            {
                _logger.LogDebug($"GetCachedDataAsync.getData: {cacheKey}");

                cachedData = await getData();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromHours(1))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(24));
                _cache.Set(cacheKey, cachedData, cacheEntryOptions);
            }
            return cachedData;
        }

        /// <summary>
        /// Invalidacion de cache, elemina los datos para todas las cacheKeys
        /// Enumeracion directa
        /// </summary>
        public void InvalidateCache()
        {
            _cache.Remove(CacheKeys.LkEjemplo);
        }

        /// <summary>
        /// Ivalidacion de una uncia cacheKey
        /// TODO: confirmar que esa cacheKey existe
        /// TODO: gestionar posibles errores
        /// </summary>
        /// <param name="cacheKey"></param>
        public void InvalidateCache(string cacheKey)
        {
            _cache.Remove(cacheKey);
        }

        #endregion


        #region Getters_datos_maestros 
        /// <summary>
        /// accede a los datos de la tabla LkEjemplo si no estan en caceh los va a buscar a bbdd
        /// TODO:  revisar la implementacion
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<LkEjemplo>> GetLkEjemploAsync()
        {
            throw new NotImplementedException();

            //return await GetCachedDataAsync(CacheKeys.LkEjemplo, async () =>
            //{
            //using (var connection = new SqlConnection(_connectionString))
            //{
            //    return await connection.QueryAsync<LkEjemplo>("SELECT IdCiclo, Ciclo, VersionPlantilla FROM Lk_CICLOS");
            //}
            //});
        }

        #endregion

        #region Busquedas
        /// <summary>
        /// Busqueda de datos en la tabla LkEjemplo a partir de datos en cache
        /// TODO:  revisar la implementacion
        /// </summary>
        /// <param name="codPlantilla"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> GetLkEjemploByIdAsync(int idEjemplo = 0)
        {
            var datos = await GetLkEjemploAsync();

            if (idEjemplo == 0) return 0;

            var idPlantilla = datos.FirstOrDefault(p => p.IdEjemplo == idEjemplo)?.IdEjemplo ?? throw new Exception($"Invalid Id: {idEjemplo}");

            return (idPlantilla);
        }
        #endregion
    }
}
