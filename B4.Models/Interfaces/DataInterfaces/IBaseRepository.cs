using B4.Models.Entities.DataEntities;

namespace B4.Models.Interfaces.DataInterfaces
{
    public interface IBaseRepository<T> where T : class
    {

        Task AddAsync();
        Task UpdateAsync();

        Task<T?> GetByIdAsync(int id);

        Task<IEnumerable<T>> GetAllAsync();
        Task DeleteAsync(int id);

    }
}
