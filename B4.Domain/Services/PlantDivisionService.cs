using B4.Models.Entities.LkEntities;
using B4.Models.RepositoryInterfaces.LkInterfaces;
using B4.Models.ServiceInterfaces;

namespace B4.Domain.Services
{
    public class PlantDivisionService : IPlantDivisionService
    {
        private readonly IPlantDivisionRepository _repo;

        public PlantDivisionService(IPlantDivisionRepository repo)
        {
            _repo = repo;
        }

        public async Task<LkPlantDivision> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<LkPlantDivision>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task AddAsync(LkPlantDivision entity)
        {
            await _repo.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                throw new Exception($"No existe división con id={id}");

            await _repo.DeleteAsync(id);
        }
    }
}
