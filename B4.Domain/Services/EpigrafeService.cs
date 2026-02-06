using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using B4.Models.Entities.LkEntities;
using B4.Models.RepositoryInterfaces.LkInterfaces;
using B4.Models.ServiceInterfaces;

namespace B4.Domain.Services
{
    public class EpigrafeService : IEpigrafeService
    {
        private readonly IEpigrafeRepository _epigrafeRepository;

        public EpigrafeService(IEpigrafeRepository epigrafeRepository)
        {
            _epigrafeRepository = epigrafeRepository;
        }

        public async Task<LkEpigrafe?> GetByIdAsync(int id)
        {
            return await _epigrafeRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<LkEpigrafe>> GetAllAsync()
        {
            return await _epigrafeRepository.GetAllAsync();
        }

        public async Task AddAsync(LkEpigrafe epigrafe)
        {
            if (epigrafe == null)
                throw new ArgumentNullException(nameof(epigrafe));

            epigrafe.CreatedAt = DateTime.UtcNow;
            epigrafe.UpdatedAt = DateTime.UtcNow;
            epigrafe.IsActive = 1;

            await _epigrafeRepository.AddAsync(epigrafe);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _epigrafeRepository.GetByIdAsync(id);

            if (existing == null)
                throw new Exception($"No existe epígrafe con id={id}");

            await _epigrafeRepository.DeleteAsync(id);
        }
    }
}
