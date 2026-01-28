using B4.Api.Middleware;
using B4.Models.Interfaces.LkInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace B4.Api.Controllers
{
    [ApiController]
    [CustomAuthorize(Role = "Admin", Permission = "can_edit")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class EpigrafeController : ControllerBase
    {
        private readonly IEpigrafeRepository _repo;

        public EpigrafeController(IEpigrafeRepository repo)
        {
            _repo = repo;
        }

        

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var data = await _repo.GetAllAsync();
            return Ok(data);

        }


    

    }
}
