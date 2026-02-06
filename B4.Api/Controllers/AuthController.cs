using B4.Data.MySQL.Repositories;
using B4.Models.Entities;
using B4.Models.RepositoryInterfaces;
using B4.Models.RepositoryInterfaces.LkInterfaces;
using B4.Shared;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            JwtService jwtService,
            IUsuarioRepository usuarioRepository,
            IPasswordHasherService passwordHasherService,
            ILogger<AuthController> logger)
        {
            _jwtService = jwtService;
            _usuarioRepository = usuarioRepository;
            _passwordHasherService = passwordHasherService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                // 0️ Validación básica
                if (request == null ||
                    string.IsNullOrWhiteSpace(request.Email) ||
                    string.IsNullOrWhiteSpace(request.Password))
                {
                    return BadRequest(new { message = "Email y contraseña son obligatorios" });
                }

                // 1️ Buscar usuario
                var usuario = await _usuarioRepository.GetByEmailAsync(request.Email);

                if (usuario == null || string.IsNullOrEmpty(usuario.HashedPassword))
                {
                    return Unauthorized(new { message = "Email o contraseña incorrectos" });
                }

                // 2️ Verificar contraseña
                bool passwordValid = _passwordHasherService.VerifyPassword(
                    usuario.HashedPassword,
                    request.Password
                );

                if (!passwordValid)
                {
                    return Unauthorized(new { message = "Email o contraseña incorrectos" });
                }

                // 3️ Generar JWT
                var token = _jwtService.GenerateToken(
                    userId: usuario.Id,
                    role: usuario.Role
                );

                // 4️ Respuesta OK
                return Ok(new { token });
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Parámetro nulo en Login");
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error de operación en Login");
                return StatusCode(500, new { message = "Error interno de autenticación" });
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error crítico en Login");
                return StatusCode(500, new
                {
                    message = "Error interno del servidor",
                    detail = ex.Message
                });
            }
        }
    }

}
