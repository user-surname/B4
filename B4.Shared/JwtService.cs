using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace B4.Shared
{
    public class JwtService
    {
        private readonly IConfiguration _config; // Inyección de configuración para leer datos de appsettings.json

        // Constructor: recibe la configuración de la aplicación
        public JwtService(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config), "La configuración no puede ser nula");
        }

        // Método que genera un token JWT para un usuario específico
        public string GenerateToken(int userId, string role)
        {
            try
            {
                if (userId <= 0)
                    throw new ArgumentException("El userId debe ser mayor que 0", nameof(userId));

                if (string.IsNullOrWhiteSpace(role))
                    throw new ArgumentException("El rol del usuario no puede estar vacío", nameof(role));

                // -------------------------------
                // 1️ Definir los "claims" del token
                // -------------------------------
                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

                // -------------------------------
                // 2️ Crear la clave secreta para firmar el token
                // -------------------------------
                var keyString = _config["Jwt:Key"];
                if (string.IsNullOrEmpty(keyString))
                    throw new InvalidOperationException("La clave JWT (Jwt:Key) no está configurada en appsettings.json");

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));

                // -------------------------------
                // 3️ Crear las credenciales de firma
                // -------------------------------
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // -------------------------------
                // 4️ Crear el token JWT
                // -------------------------------
                var issuer = _config["Jwt:Issuer"];
                var audience = _config["Jwt:Audience"];
                var expireMinutesString = _config["Jwt:ExpireMinutes"];

                if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(expireMinutesString))
                    throw new InvalidOperationException("Issuer, Audience o ExpireMinutes no están configurados en appsettings.json");

                if (!int.TryParse(expireMinutesString, out int expireMinutes) || expireMinutes <= 0)
                    throw new InvalidOperationException("ExpireMinutes debe ser un número entero mayor que 0");

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                    signingCredentials: creds
                );

                // -------------------------------
                // 5️ Devolver el token en formato string
                // -------------------------------
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                // Aquí podrías loguear el error con NLog o cualquier logger que uses
                throw new InvalidOperationException("Error generando el token JWT", ex);
            }
        }
    }


}
