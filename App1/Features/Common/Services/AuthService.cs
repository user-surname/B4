using System.Net.Http.Json;
using App1.Features.Auth.Models;
using App1.Features.Common.Interfaces;

namespace App1.Features.Common.Services;

public sealed class AuthService(IHttpClientFactory httpClientFactory, ITokenStore tokenStore) : IAuthService
{
    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient("AuthApi");
        var response = await client.PostAsJsonAsync("auth/login", request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken: cancellationToken);
        if (payload is null || string.IsNullOrWhiteSpace(payload.Token))
        {
            throw new InvalidOperationException("No se recibió token JWT.");
        }

        tokenStore.SetToken(payload.Token);
        return payload;
    }
}
