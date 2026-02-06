using B4.Models.Entities.LkEntities;
using B4.Models.RepositoryInterfaces.LkInterfaces;
using B4.Models.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Domain.Services
{
    public class CiclosService : ICiclosService
    {
        private readonly ICiclosRepository _ciclosRepository;

        public CiclosService(ICiclosRepository ciclosRepository)
        {
            _ciclosRepository = ciclosRepository;
        }

        public async Task<LkCiclos?> GetByIdAsync(int id)
        {
            return await _ciclosRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<LkCiclos>> GetAllAsync()
        {
            return await _ciclosRepository.GetAllAsync();
        }

        public async Task AddAsync(LkCiclos ciclo)
        {
            if (ciclo == null)
                throw new ArgumentNullException(nameof(ciclo));

            ciclo.CreatedAt = DateTime.UtcNow;
            ciclo.UpdatedAt = DateTime.UtcNow;
            ciclo.IsActive = 1;

            await _ciclosRepository.AddAsync(ciclo);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _ciclosRepository.GetByIdAsync(id);

            if (existing == null)
                throw new Exception($"No existe ciclo con id={id}");

            await _ciclosRepository.DeleteAsync(id);
        }
    }
}
