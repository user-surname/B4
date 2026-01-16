using B4.Models.Entities;
using B4.Shared;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]  // Ruta base para todo este controller
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]  // POST /api/auth/login
        public IActionResult Login(Models.Entities.LoginRequest request)
        {
            try
            {
                // 1. Validar usuario contra la base de datos
                if (request.Email != "ejemploemail@eamail.com" || request.Password != "1234")
                {
                    // Devuelve 401 Unauthorized si las credenciales no coinciden
                    return Unauthorized(new { message = "Email o contraseña incorrectos" });
                }

                // 2. Generar token JWT
                var token = _jwtService.GenerateToken(userId: 1, role: "Admin");

                // 3. Devolver token al cliente
                return Ok(new { token });
            }
            catch (ArgumentNullException ex)
            {
                // Captura errores específicos, por ejemplo valores nulos
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Captura cualquier otro error inesperado
                return StatusCode(500, new { message = "Ocurrió un error al procesar la solicitud", detail = ex.Message });
            }
        }
    }
}
