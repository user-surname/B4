using System.Net.Http.Headers;
using B4.WinUI.Features.Common.Interfaces;

namespace B4.WinUI.Features.Common.Services;

public sealed class AuthHttpMessageHandler(ITokenStore tokenStore) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = tokenStore.GetToken();
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
