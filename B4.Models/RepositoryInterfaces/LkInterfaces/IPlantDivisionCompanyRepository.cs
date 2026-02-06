using System.Collections.Generic;
using System.Threading.Tasks;
using B4.Models.Entities.LkEntities;

namespace B4.Models.RepositoryInterfaces.LkInterfaces

{
    // Hereda de IRepository<Control> para tener los métodos CRUD básicos
    public interface IPlantDivisionCompanyRepository : IRepository<LkPlantDivisionCompany>
    {

        // Métodos específicos que no son genéricos

    }
}
