using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Shared;

namespace Client.Shared;

/// <summary>
/// Методы расширения для построителя хоста WebAssembly.
/// </summary>
public static class WebAssemblyHostBuilderExtensions
{
    /// <summary>
    /// Построить хост с авторизацией.
    /// </summary>
    /// <param name="builder">Построитель хоста WebAssembly.</param>
    /// <returns>Хост WebAssembly.</returns>
    public static async Task<WebAssemblyHost> BuildWithAuthorizationAsync(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddScoped<TokenRepository>();
        
        var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
        builder.Services.AddScoped(sp => httpClient);

        var configurationOptions = await httpClient.GetFromJsonAsync<ConfigurationOptions>(RouteConstants.ConfigurationData.GetOptionsUrl());
        if (configurationOptions is null)
        {
            throw new InvalidOperationException("Not able to load configuration.");
        }
        
        // Регистрация сервиса аутентификации
        builder.Services.AddScoped<AuthService>(sp =>
            new AuthService(
                httpClient: sp.GetRequiredService<HttpClient>(),
                tokenRepository: sp.GetRequiredService<TokenRepository>(),
                navigation: sp.GetRequiredService<NavigationManager>(),
                authServicePath: configurationOptions.AuthenticationServerUrl
            ));

        builder.Services.AddAuthorizationCore();
        
        var host = builder.Build();

        return host;
    }
}
