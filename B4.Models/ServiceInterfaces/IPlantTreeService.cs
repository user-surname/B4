using B4.Models.Entities.LkEntities;

namespace B4.Models.ServiceInterfaces
{
    public interface IPlantTreeService
    {
        Task<LkPlantTree> GetByIdAsync(int id);
        Task<IEnumerable<LkPlantTree>> GetAllAsync();
        Task AddAsync(LkPlantTree entity);
        Task DeleteAsync(int id);
    }
}
