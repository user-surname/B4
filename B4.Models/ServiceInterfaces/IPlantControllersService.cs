using B4.Models.Entities.LkEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.ServiceInterfaces
{
    public interface IPlantControllersService
    {

        Task AddAsync(LkPlantControllers entity);
        Task<LkPlantControllers?> GetByIdAsync(int id);
        Task<IEnumerable<LkPlantControllers>> GetAllAsync();
        Task DeleteAsync(int id);

    }
}
