using App1.Features.Common.Interfaces;
using App1.Features.Common.Services;
using App1.Features.Controllers.ViewModels;
using App1.Features.Auth.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace App1.Features.Common.Extensions.DI;

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
