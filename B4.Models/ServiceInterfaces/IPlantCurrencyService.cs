using B4.Models.Entities.LkEntities;

namespace B4.Models.ServiceInterfaces
{
    public interface IPlantCurrencyService
    {
        Task<LkPlantCurrency> GetByIdAsync(int id);
        Task<IEnumerable<LkPlantCurrency>> GetAllAsync();
        Task AddAsync(LkPlantCurrency entity);
        Task DeleteAsync(int id);
    }
}
