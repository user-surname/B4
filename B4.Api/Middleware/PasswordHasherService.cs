using B4.Models.RepositoryInterfaces.LkInterfaces;
using Microsoft.AspNetCore.Identity;

namespace B4.Api.Middleware
{
    public class PasswordHasherService : IPasswordHasherService
    {
        private readonly PasswordHasher<object> _passwordHasher;

        public PasswordHasherService()
        {
            _passwordHasher = new PasswordHasher<object>();
        }

        // Hashear la contraseña (para registrar usuario)
        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }

        // Verificar contraseña (para login)
        public bool VerifyPassword(string hashedPassword, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}

