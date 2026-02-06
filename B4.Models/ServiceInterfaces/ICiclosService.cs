using B4.Models.Entities.LkEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.ServiceInterfaces
{
    public interface ICiclosService
    {
        Task<LkCiclos?> GetByIdAsync(int id);
        Task<IEnumerable<LkCiclos>> GetAllAsync();
        Task AddAsync(LkCiclos ciclo);
        Task DeleteAsync(int id);
    }
}
