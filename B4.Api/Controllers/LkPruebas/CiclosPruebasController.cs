using B4.Models.Entities.LkEntities;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers.Lk
{
    public class CiclosController : LkControllerBase
    {
        public CiclosController(IMemoryCacheService cache, ILogger<CiclosController> logger)
            : base(cache, logger) { }

        // GET /api/v1/lk/ciclos?onlyActive=true
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<LkCiclos>), StatusCodes.Status200OK)]
        public Task<IActionResult> GetAll([FromQuery] bool onlyActive = true)
            => OkList(_cache.GetCiclosAsync, onlyActive);
    }
}
