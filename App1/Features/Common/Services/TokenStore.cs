using App1.Features.Common.Interfaces;

namespace App1.Features.Common.Services;

public sealed class TokenStore : ITokenStore
{
    private string? _token;

    public string? GetToken() => _token;

    public void SetToken(string? token) => _token = token;

    public bool HasToken => !string.IsNullOrWhiteSpace(_token);
}
