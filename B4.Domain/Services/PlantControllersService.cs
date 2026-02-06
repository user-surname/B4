using B4.Models.Entities.LkEntities;
using B4.Models.RepositoryInterfaces.LkInterfaces;
using B4.Models.ServiceInterfaces;

namespace B4.Domain.Services
{
    public class PlantControllersService : IPlantControllersService
    {
        private readonly IPlantControllersRepository _repo;

        public PlantControllersService(IPlantControllersRepository repo)
        {
            _repo = repo;
        }

        public async Task<LkPlantControllers> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<LkPlantControllers>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task AddAsync(LkPlantControllers entity)
        {
            await _repo.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                throw new Exception($"No existe PlantController con id={id}");

            await _repo.DeleteAsync(id);
        }
    }
}
