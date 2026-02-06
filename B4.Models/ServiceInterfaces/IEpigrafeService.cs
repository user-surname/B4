using System.Collections.Generic;
using System.Threading.Tasks;
using B4.Models.Entities.LkEntities;

namespace B4.Models.ServiceInterfaces
{
    public interface IEpigrafeService
    {
        Task<LkEpigrafe?> GetByIdAsync(int id);
        Task<IEnumerable<LkEpigrafe>> GetAllAsync();
        Task AddAsync(LkEpigrafe epigrafe);
        Task DeleteAsync(int id);
    }
}
