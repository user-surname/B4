namespace App1.Features.Common.Interfaces;

public interface ITokenStore
{
    string? GetToken();
    void SetToken(string? token);
    bool HasToken { get; }
}
