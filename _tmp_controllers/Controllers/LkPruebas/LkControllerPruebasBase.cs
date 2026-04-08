using B4.Api.Middleware;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers.Lk
{
    [ApiController]
    [CustomAuthorize]
    [Authorize]
    [Route("api/v1/lk/[controller]")]
    public abstract class LkControllerBase : ControllerBase
    {
        protected readonly IMemoryCacheService _cache;

        protected LkControllerBase(IMemoryCacheService cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Método común para devolver listas de tablas maestras (LK).
        /// Recibe una función que obtiene los datos (normalmente desde caché)
        /// y devuelve una respuesta HTTP 200 (OK) con la lista resultante.
        /// Se usa para evitar repetir la misma lógica en todos los controllers LK.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getter"></param>
        /// <param name="onlyActive"></param>
        /// <returns></returns>
        protected async Task<IActionResult> OkList<T>(Func<bool, Task<IReadOnlyList<T>>> getter, bool onlyActive)
        {
            var data = await getter(onlyActive);
            return Ok(data);
        }
    }
}
