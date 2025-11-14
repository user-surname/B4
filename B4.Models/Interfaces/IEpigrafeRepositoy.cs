using System.Collections.Generic;
using System.Threading.Tasks;
using B4.Models.Entities;

namespace B4.Models.Interfaces

{
    // Hereda de IRepository<Epigrafe> para tener los métodos CRUD básicos
    public interface IEpigrafeRepository : IRepository<Epigrafe>
    {
        // Métodos específicos que no son genéricos

        /*
        Task<Epigrafe?> GetByEmailAsync(string email);
        Task<IEnumerable<Epigrafe>> GetUsersByRoleAsync(string role);
        Task<IEnumerable<Epigrafe>> FindWithOrdersAsync(int userId);
        */

    }
}