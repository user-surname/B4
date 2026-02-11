using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces; // ajusta si usáis otro namespace
using B4.Models.ServiceInterfaces;

namespace B4.Domain.Services;

public class DataComentariosService : IDataComentariosService
{
    private readonly IDataComentariosRepository _repo;

    public DataComentariosService(IDataComentariosRepository repo)
    {
        _repo = repo;
    }

    public Task<DataComentarios?> GetByIdAsync(int id)
        => _repo.GetByIdAsync(id);

    public Task<IEnumerable<DataComentarios>> GetAllAsync()
        => _repo.GetAllAsync();

    public async Task AddAsync(DataComentarios entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        // Defaults técnicos (como en otros)
        entity.GuidCarga = Guid.NewGuid();
        entity.FechaUltModif = DateTime.UtcNow;
        entity.IdAPICarga = 1;

        await _repo.AddAsync(entity);
    }

    public async Task UpdateAsync(DataComentarios entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        entity.FechaUltModif = DateTime.UtcNow;
        await _repo.UpdateAsync(entity);
    }

    public Task DeleteAsync(int id)
        => _repo.DeleteAsync(id);
}
