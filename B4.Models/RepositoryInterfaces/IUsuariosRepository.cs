using B4.Models.Entities;
using B4.Models.Entities.DataEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace B4.Models.RepositoryInterfaces

{
    // Hereda de IRepository<Control> para tener los métodos CRUD básicos
    public interface IUsuariosRepository : IRepository<Entities.DataEntities.Usuario>
    {

        // Métodos específicos que no son genéricos

    }
}
