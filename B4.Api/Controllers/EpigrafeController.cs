using Microsoft.AspNetCore.Mvc;
using B4.Models.Interfaces.LkInterfaces;

namespace B4.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EpigrafeController : ControllerBase
    {
        private readonly IEpigrafeRepository _repo;

        public EpigrafeController(IEpigrafeRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _repo.GetAllAsync();
            return Ok(data);
        }
    }
}
