using B4.Models.Entities;

namespace B4.Models.ServiceInterfaces
{
    /// <summary>
    /// Exposes application services for ControlPlanta queries.
    /// </summary>
    public interface IControlPlantaService
    {
        /// <summary>
        /// Retrieves a ControlPlanta by identifier.
        /// </summary>
        /// <param name="id">Identifier to search.</param>
        /// <returns>The matching entity or null.</returns>
        Task<ControlPlanta?> GetByIdAsync(int id);

        /// <summary>
        /// Retrieves all ControlPlanta records.
        /// </summary>
        /// <returns>Collection of entities.</returns>
        Task<IEnumerable<ControlPlanta>> GetAllAsync();
    }
}
