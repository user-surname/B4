using System.Collections.Generic;
using System.Threading.Tasks;
using B4.Models.Entities;

namespace B4.Models.Interfaces

{
    // Hereda de IRepository<Control> para tener los métodos CRUD básicos
    public interface IPlantCountryRepository : IRepository<LkPlantCountry>
    {

        // Métodos específicos que no son genéricos

    }
}
