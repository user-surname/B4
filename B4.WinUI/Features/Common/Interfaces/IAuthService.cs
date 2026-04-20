using B4.WinUI.Features.Auth.Models;

namespace B4.WinUI.Features.Common.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
