using B4.Models.Entities.LkEntities;

namespace B4.Models.ServiceInterfaces
{
    public interface IPlantCountryService
    {
        Task<LkPlantCountry> GetByIdAsync(int id);
        Task<IEnumerable<LkPlantCountry>> GetAllAsync();
        Task AddAsync(LkPlantCountry entity);
        Task DeleteAsync(int id);
    }
}
