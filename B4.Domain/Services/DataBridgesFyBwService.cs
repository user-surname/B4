using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces; // ajusta si tu namespace es otro
using B4.Models.ServiceInterfaces;

namespace B4.Domain.Services;

public class DataBridgesFyBwService : IDataBridgesFyBwService
{
    private readonly IDataBridgesFyBwRepository _repo;

    public DataBridgesFyBwService(IDataBridgesFyBwRepository repo)
    {
        _repo = repo;
    }

    public Task<DataBridgesFyBw?> GetByIdAsync(int id)
        => _repo.GetByIdAsync(id);

    public Task<IEnumerable<DataBridgesFyBw>> GetAllAsync()
        => _repo.GetAllAsync();
}
