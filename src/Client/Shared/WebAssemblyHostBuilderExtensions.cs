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
        builder.Services.AddSingleton<AuthStateProvider>();
        builder.Services.AddSingleton<AuthenticationStateProvider>(provider => provider.GetRequiredService<AuthStateProvider>());
        
        var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
        builder.Services.AddScoped(sp => httpClient);

        var configurationOptions = await httpClient.GetFromJsonAsync<ConfigurationOptions>(RouteConstants.ConfigurationData.Options);
        if (configurationOptions is null)
        {
            throw new InvalidOperationException("Не удалось загрузить конфигурацию.");
        }
        
        // Регистрация сервиса аутентификации
        builder.Services.AddScoped<AuthService>(sp =>
            new AuthService(
                authStateProvider: sp.GetRequiredService<AuthStateProvider>(),
                tokenRepository: sp.GetRequiredService<TokenRepository>(),
                navigation: sp.GetRequiredService<NavigationManager>(),
                authServicePath: configurationOptions.AuthenticationServerUrl
            ));

        builder.Services.AddAuthorizationCore();
        
        var host = builder.Build();
        await host.Services.GetRequiredService<AuthStateProvider>().InitializeAuthenticationStateAsync();

        return host;
    }
}
