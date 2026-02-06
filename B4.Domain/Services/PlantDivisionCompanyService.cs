using B4.Models.Entities.LkEntities;
using B4.Models.RepositoryInterfaces.LkInterfaces;
using B4.Models.ServiceInterfaces;

namespace B4.Domain.Services
{
    public class PlantDivisionCompanyService : IPlantDivisionCompanyService
    {
        private readonly IPlantDivisionCompanyRepository _repo;

        public PlantDivisionCompanyService(IPlantDivisionCompanyRepository repo)
        {
            _repo = repo;
        }

        public async Task<LkPlantDivisionCompany> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<LkPlantDivisionCompany>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task AddAsync(LkPlantDivisionCompany entity)
        {
            await _repo.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                throw new Exception($"No existe division company con id={id}");

            await _repo.DeleteAsync(id);
        }
    }
}
