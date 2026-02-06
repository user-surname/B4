using B4.Models.Entities.LkEntities;
using B4.Models.RepositoryInterfaces.LkInterfaces;
using B4.Models.ServiceInterfaces;

namespace B4.Domain.Services
{
    public class PlantSubdivisionService : IPlantSubdivisionService
    {
        private readonly IPlantSubdivisionRepository _repository;

        public PlantSubdivisionService(IPlantSubdivisionRepository repository)
        {
            _repository = repository;
        }

        public async Task<LkPlantSubdivision> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<LkPlantSubdivision>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task AddAsync(LkPlantSubdivision entity)
        {
            await _repository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new Exception($"No existe subdivisión con id={id}");

            await _repository.DeleteAsync(id);
        }
    }
}
