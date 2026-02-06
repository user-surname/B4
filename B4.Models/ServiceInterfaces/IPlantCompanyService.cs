using System.Collections.Generic;
using System.Threading.Tasks;
using B4.Models.Entities.LkEntities;

namespace B4.Models.ServiceInterfaces
{
    public interface IPlantCompanyService
    {
        Task AddAsync(LkPlantCompany entity);
        Task<LkPlantCompany?> GetByIdAsync(int id);
        Task<IEnumerable<LkPlantCompany>> GetAllAsync();
        Task DeleteAsync(int id);
    }
}
