using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Nodes;
using B4.WinUI.Features.Common.Interfaces;

namespace B4.WinUI.Features.Common.Services;

public sealed class ApiCrudService(IHttpClientFactory httpClientFactory) : IApiCrudService
{
    public async Task<JsonNode?> GetAllAsync(string routeSegment, CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient("B4Api");
        var node = await client.GetFromJsonAsync<JsonNode>(routeSegment, cancellationToken);
        return node;
    }

    public async Task<JsonNode?> GetByIdAsync(string routeSegment, int id, CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient("B4Api");
        var node = await client.GetFromJsonAsync<JsonNode>($"{routeSegment}/{id}", cancellationToken);
        return node;
    }

    public async Task<JsonNode?> CreateAsync(string routeSegment, string rawJson, CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient("B4Api");
        using var content = new StringContent(rawJson, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(routeSegment, content, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<JsonNode>(cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(string routeSegment, int id, CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient("B4Api");
        var response = await client.DeleteAsync($"{routeSegment}/{id}", cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}
