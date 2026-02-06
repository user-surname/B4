using B4.Models.Entities.LkEntities;

namespace B4.Models.ServiceInterfaces
{
    public interface IPlantillasBotonesPasosTiposService
    {
        Task<LkPlantillasBotonesPasosTipos> GetByIdAsync(int id);
        Task<IEnumerable<LkPlantillasBotonesPasosTipos>> GetAllAsync();
        Task AddAsync(LkPlantillasBotonesPasosTipos entity);
        Task DeleteAsync(int id);
    }
}
