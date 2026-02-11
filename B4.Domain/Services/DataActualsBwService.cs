using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces; // ajusta si tu namespace es otro
using B4.Models.ServiceInterfaces;

namespace B4.Domain.Services;

public class DataActualsBwService : IDataActualsBwService
{
    private readonly IDataActualsBwRepository _repo;

    public DataActualsBwService(IDataActualsBwRepository repo)
    {
        _repo = repo;
    }

    public Task<DataActualsBw?> GetByIdAsync(int id)
        => _repo.GetByIdAsync(id);

    public Task<IEnumerable<DataActualsBw>> GetAllAsync()
        => _repo.GetAllAsync();
}
