using System.Collections.Generic;
using System.Threading.Tasks;
using B4.Models.Entities;

namespace B4.Models.Interfaces

{
    // Hereda de IRepository<Control> para tener los métodos CRUD básicos
    public interface ICiclosRepository : IRepository<LkCiclos>
    {

        // Métodos específicos que no son genéricos

    }
}
