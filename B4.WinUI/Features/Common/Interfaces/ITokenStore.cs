namespace B4.WinUI.Features.Common.Interfaces;

public interface ITokenStore
{
    string? GetToken();
    void SetToken(string? token);
    bool HasToken { get; }
}
