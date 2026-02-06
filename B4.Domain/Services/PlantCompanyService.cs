using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using B4.Models.Entities.LkEntities;
using B4.Models.RepositoryInterfaces.LkInterfaces;
using B4.Models.ServiceInterfaces;

namespace B4.Domain.Services
{
    public class PlantCompanyService : IPlantCompanyService
    {
        private readonly IPlantCompanyRepository _repository;

        public PlantCompanyService(IPlantCompanyRepository repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(LkPlantCompany entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = 1;

            await _repository.AddAsync(entity);
        }

        public async Task<LkPlantCompany?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<LkPlantCompany>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task UpdateAsync(LkPlantCompany entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new Exception($"No existe PlantCompany con id={id}");

            await _repository.DeleteAsync(id);
        }
    }
}
