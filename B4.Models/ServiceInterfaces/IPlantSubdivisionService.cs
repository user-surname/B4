using B4.Models.Entities.LkEntities;

namespace B4.Models.ServiceInterfaces
{
    public interface IPlantSubdivisionService
    {
        Task<LkPlantSubdivision> GetByIdAsync(int id);
        Task<IEnumerable<LkPlantSubdivision>> GetAllAsync();
        Task AddAsync(LkPlantSubdivision entity);
        Task DeleteAsync(int id);
    }
}
