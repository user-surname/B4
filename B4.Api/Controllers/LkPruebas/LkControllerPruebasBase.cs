using B4.Api.Middleware;
using B4.Models.Interfaces.LkInterfaces;
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
        /// 
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
