using System.Collections.Generic;
using System.Threading.Tasks;
using B4.Models.Entities.LkEntities;

namespace B4.Models.ServiceInterfaces
{
    public interface IFasesService
    {
        Task AddAsync(LkFases entity);
        Task<LkFases?> GetByIdAsync(int id);
        Task<IEnumerable<LkFases>> GetAllAsync();
        Task DeleteAsync(int id);
    }
}
