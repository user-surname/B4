using B4.Data.MySQL.Repositories;
using B4.Models.Entities;
using B4.Models.RepositoryInterfaces;
using B4.Models.RepositoryInterfaces.LkInterfaces;
using B4.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers
{
    [AllowAnonymous]
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
            _logger.LogInformation("Login attempt received for email: {Email}", request?.Email ?? "null");

            try
            {
                // 0️ Validación básica
                if (request == null ||
                    string.IsNullOrWhiteSpace(request.Email) ||
                    string.IsNullOrWhiteSpace(request.Password))
                {
                    _logger.LogWarning("Login rejected: missing email or password. Email provided: {EmailProvided}", request?.Email != null);
                    return BadRequest(new { message = "Email y contraseña son obligatorios" });
                }

                // 1️ Buscar usuario
                _logger.LogDebug("Looking up user by email: {Email}", request.Email);
                var usuario = await _usuarioRepository.GetByEmailAsync(request.Email);

                if (usuario == null || string.IsNullOrEmpty(usuario.HashedPassword))
                {
                    _logger.LogWarning("Login failed: user not found or has no password. Email: {Email}", request.Email);
                    return Unauthorized(new { message = "Email o contraseña incorrectos" });
                }

                _logger.LogDebug("User found. UserId: {UserId}. Verifying password...", usuario.Id);

                // 2️ Verificar contraseña
                bool passwordValid = _passwordHasherService.VerifyPassword(
                    usuario.HashedPassword,
                    request.Password
                );

                if (!passwordValid)
                {
                    _logger.LogWarning("Login failed: invalid password for UserId: {UserId}", usuario.Id);
                    return Unauthorized(new { message = "Email o contraseña incorrectos" });
                }

                // 3️ Generar JWT
                _logger.LogDebug("Password verified. Generating JWT for UserId: {UserId}, Role: {Role}", usuario.Id, usuario.Role);
                var token = _jwtService.GenerateToken(
                    userId: usuario.Id,
                    role: usuario.Role
                );

                // 4️ Respuesta OK
                _logger.LogInformation("Login successful for UserId: {UserId}, Role: {Role}", usuario.Id, usuario.Role);
                return Ok(new { token });
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Login failed: null argument. Email: {Email}", request?.Email);
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Login failed: invalid operation. Email: {Email}", request?.Email);
                return StatusCode(500, new { message = "Error interno de autenticación" });
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Login failed: unhandled exception. Email: {Email}", request?.Email);
                return StatusCode(500, new
                {
                    message = "Error interno del servidor",
                    detail = ex.Message
                });
            }
        }
    }
}