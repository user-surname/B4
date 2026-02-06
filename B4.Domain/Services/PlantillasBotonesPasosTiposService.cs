using B4.Models.Entities.LkEntities;
using B4.Models.RepositoryInterfaces.LkInterfaces;
using B4.Models.ServiceInterfaces;

namespace B4.Domain.Services
{
    public class PlantillasBotonesPasosTiposService : IPlantillasBotonesPasosTiposService
    {
        private readonly IPlantillasBotonesPasosTiposRepository _repo;

        public PlantillasBotonesPasosTiposService(IPlantillasBotonesPasosTiposRepository repo)
        {
            _repo = repo;
        }

        public async Task<LkPlantillasBotonesPasosTipos> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<LkPlantillasBotonesPasosTipos>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task AddAsync(LkPlantillasBotonesPasosTipos entity)
        {
            await _repo.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                throw new Exception($"No existe paso tipo con id={id}");

            await _repo.DeleteAsync(id);
        }
    }
}
