using B4.Models.Entities;
using B4.Models.RepositoryInterfaces;
using B4.Models.ServiceInterfaces;

namespace B4.Domain.Services
{
    /// <summary>
    /// Provides application operations for ControlPlanta.
    /// </summary>
    public class ControlPlantaService : IControlPlantaService
    {
        private readonly IControlPlantaRepository _controlPlantaRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlPlantaService"/> class.
        /// </summary>
        /// <param name="controlPlantaRepository">ControlPlanta repository.</param>
        public ControlPlantaService(IControlPlantaRepository controlPlantaRepository)
        {
            _controlPlantaRepository = controlPlantaRepository;
        }

        /// <summary>
        /// Retrieves a ControlPlanta by identifier.
        /// </summary>
        /// <param name="id">Identifier to search.</param>
        /// <returns>The matching entity or null.</returns>
        public async Task<ControlPlanta?> GetByIdAsync(int id)
        {
            return await _controlPlantaRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Retrieves all ControlPlanta records.
        /// </summary>
        /// <returns>Collection of entities.</returns>
        public async Task<IEnumerable<ControlPlanta>> GetAllAsync()
        {
            return await _controlPlantaRepository.GetAllAsync();
        }
    }
}
