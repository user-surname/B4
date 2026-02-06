using B4.Models.Entities.LkEntities;

namespace B4.Models.ServiceInterfaces
{
    public interface IPlantDivisionCompanyService
    {
        Task<LkPlantDivisionCompany> GetByIdAsync(int id);
        Task<IEnumerable<LkPlantDivisionCompany>> GetAllAsync();
        Task AddAsync(LkPlantDivisionCompany entity);
        Task DeleteAsync(int id);
    }
}
