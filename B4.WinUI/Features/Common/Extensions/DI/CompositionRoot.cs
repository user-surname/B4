using B4.WinUI.Features.Auth.ViewModels;
using B4.WinUI.Features.Common.Interfaces;
using B4.WinUI.Features.Common.Services;
using B4.WinUI.Features.Controllers.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace B4.WinUI.Features.Common.Extensions.DI;

public static class CompositionRoot
{
    public static IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
            {
                const string apiVersion = "v1";
                const string baseUrl = "http://localhost:5029/api/";

                services.AddSingleton<ITokenStore, TokenStore>();
                services.AddTransient<AuthHttpMessageHandler>();

                services.AddHttpClient("AuthApi", c =>
                {
                    c.BaseAddress = new Uri($"{baseUrl}{apiVersion}/");
                });

                services.AddHttpClient("B4Api", c =>
                {
                    c.BaseAddress = new Uri($"{baseUrl}{apiVersion}/");
                }).AddHttpMessageHandler<AuthHttpMessageHandler>();

                services.AddSingleton<IAuthService, AuthService>();
                services.AddSingleton<IApiMetadataService, ApiMetadataService>();
                services.AddSingleton<IApiCrudService, ApiCrudService>();

                services.AddTransient<LoginViewModel>();
                services.AddTransient<ControllersViewModel>();
            });
    }
}
