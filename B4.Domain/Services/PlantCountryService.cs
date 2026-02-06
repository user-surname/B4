using B4.Models.Entities.LkEntities;
using B4.Models.RepositoryInterfaces.LkInterfaces;
using B4.Models.ServiceInterfaces;

namespace B4.Domain.Services
{
    public class PlantCountryService : IPlantCountryService
    {
        private readonly IPlantCountryRepository _repo;

        public PlantCountryService(IPlantCountryRepository repo)
        {
            _repo = repo;
        }

        public async Task<LkPlantCountry> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<LkPlantCountry>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task AddAsync(LkPlantCountry entity)
        {
            await _repo.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                throw new Exception($"No existe país con id={id}");

            await _repo.DeleteAsync(id);
        }
    }
}
