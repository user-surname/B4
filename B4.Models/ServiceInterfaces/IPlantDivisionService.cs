using B4.Models.Entities.LkEntities;

namespace B4.Models.ServiceInterfaces
{
    public interface IPlantDivisionService
    {
        Task<LkPlantDivision> GetByIdAsync(int id);
        Task<IEnumerable<LkPlantDivision>> GetAllAsync();
        Task AddAsync(LkPlantDivision entity);
        Task DeleteAsync(int id);
    }
}
