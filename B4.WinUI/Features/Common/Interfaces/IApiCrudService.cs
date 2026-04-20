using System.Text.Json.Nodes;

namespace B4.WinUI.Features.Common.Interfaces;

public interface IApiCrudService
{
    Task<JsonNode?> GetAllAsync(string routeSegment, CancellationToken cancellationToken = default);
    Task<JsonNode?> GetByIdAsync(string routeSegment, int id, CancellationToken cancellationToken = default);
    Task<JsonNode?> CreateAsync(string routeSegment, string rawJson, CancellationToken cancellationToken = default);
    Task DeleteAsync(string routeSegment, int id, CancellationToken cancellationToken = default);
}
