using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces;
using B4.Models.ServiceInterfaces;

namespace B4.Domain.Services;

public class DataBridgesFyBwEurService : IDataBridgesFyBwEurService
{
    private readonly IDataBridgesFyBwEurRepository _repo;

    public DataBridgesFyBwEurService(IDataBridgesFyBwEurRepository repo)
    {
        _repo = repo;
    }

    public Task<DataBridgesFyBwEur?> GetByIdAsync(int id)
        => _repo.GetByIdAsync(id);

    public Task<IEnumerable<DataBridgesFyBwEur>> GetAllAsync()
        => _repo.GetAllAsync();
}
