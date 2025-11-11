using B4.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

// Hereda de IRepository<Usuario> para tener los métodos CRUD básicos
public interface IUsuarioRepository : IRepository<Usuario>
{
    // Métodos específicos que no son genéricos
    Task<Usuario?> GetByEmailAsync(string email);
    Task<IEnumerable<Usuario>> GetUsersByRoleAsync(string role);
    Task<IEnumerable<Usuario>> FindWithOrdersAsync(int userId);
}
