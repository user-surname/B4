using B4.Models.Entities;
using B4.Models.Entities.LkEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.ServiceInterfaces
{
    public interface IControlService
    {
        Task<Control?> GetByIdAsync(int id);
        Task<IEnumerable<Control>> GetAllAsync();
        Task AddAsync(Control Control);
        Task DeleteAsync(int id);
    }
}
