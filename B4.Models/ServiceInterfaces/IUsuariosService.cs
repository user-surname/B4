using B4.Models.Entities.DataEntities;
using B4.Models.Entities.LkEntities;

namespace B4.Models.ServiceInterfaces
{
    public interface IUsuariosService
    {
        Task<Usuario> GetByEmailAsync(string email);

        Task<Usuario> GetByIdAsync(int id);
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task AddAsync(Usuario entity);
        Task DeleteAsync(int id);
    }
}
