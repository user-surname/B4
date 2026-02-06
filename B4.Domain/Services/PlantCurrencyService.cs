using B4.Models.Entities.LkEntities;
using B4.Models.RepositoryInterfaces.LkInterfaces;
using B4.Models.ServiceInterfaces;

namespace B4.Domain.Services
{
    public class PlantCurrencyService : IPlantCurrencyService
    {
        private readonly IPlantCurrencyRepository _repo;

        public PlantCurrencyService(IPlantCurrencyRepository repo)
        {
            _repo = repo;
        }

        public async Task<LkPlantCurrency> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<LkPlantCurrency>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task AddAsync(LkPlantCurrency entity)
        {
            await _repo.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                throw new Exception($"No existe currency con id={id}");

            await _repo.DeleteAsync(id);
        }
    }
}
