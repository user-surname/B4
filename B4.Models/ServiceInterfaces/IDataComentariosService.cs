using B4.Models.Entities.DataEntities;

namespace B4.Models.ServiceInterfaces;

public interface IDataComentariosService
{
    Task<DataComentarios?> GetByIdAsync(int id);
    Task<IEnumerable<DataComentarios>> GetAllAsync();

    Task AddAsync(DataComentarios entity);
    Task UpdateAsync(DataComentarios entity);
    Task DeleteAsync(int id);
}
