using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using B4.Models.Entities.LkEntities;
using B4.Models.RepositoryInterfaces.LkInterfaces;
using B4.Models.ServiceInterfaces;

namespace B4.Domain.Services
{
    public class FasesService : IFasesService
    {
        private readonly IFasesRepository _fasesRepository;

        public FasesService(IFasesRepository fasesRepository)
        {
            _fasesRepository = fasesRepository;
        }

        public async Task AddAsync(LkFases entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = 1;

            await _fasesRepository.AddAsync(entity);
        }

        public async Task<LkFases?> GetByIdAsync(int id)
        {
            return await _fasesRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<LkFases>> GetAllAsync()
        {
            return await _fasesRepository.GetAllAsync();
        }

        public async Task UpdateAsync(LkFases entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            await _fasesRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _fasesRepository.GetByIdAsync(id);
            if (existing == null)
                throw new Exception($"No existe fase con id={id}");

            await _fasesRepository.DeleteAsync(id);
        }
    }
}
