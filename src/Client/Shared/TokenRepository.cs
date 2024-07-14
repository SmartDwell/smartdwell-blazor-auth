using System.Reflection;
using Blazored.LocalStorage;

namespace Client.Shared;

/// <summary>
/// Репозиторий токенов.
/// </summary>
public class TokenRepository
{
    private readonly ILocalStorageService _localStorage;
    private readonly string _accessTokenKey;
    private readonly string _refreshTokenKey;

    /// <summary>
    /// Конструктор класса <see cref="TokenRepository"/>.
    /// </summary>
    /// <param name="localStorage">Сервис локального хранилища.</param>
    public TokenRepository(ILocalStorageService localStorage)
    {
        var systemName = Assembly.GetEntryAssembly()?.FullName?.Split(',')[0] ?? "DefaultSystem";
        _accessTokenKey = $"{systemName}.AccessToken";
        _refreshTokenKey = $"{systemName}.RefreshToken";
        
        _localStorage = localStorage;
    }

    /// <summary>
    /// Установить токены.
    /// </summary>
    /// <param name="accessToken">Токен доступа.</param>
    /// <param name="refreshToken">Токен перевыпуска.</param>
    public async Task SetTokensAsync(string accessToken, string refreshToken)
    {
        await _localStorage.SetItemAsync(_accessTokenKey, accessToken);
        await _localStorage.SetItemAsync(_refreshTokenKey, refreshToken);
    }

    /// <summary>
    /// Получить токен доступа.
    /// </summary>
    /// <returns>Токен доступа.</returns>
    public async Task<string?> GetAccessTokenAsync()
    {
        return await _localStorage.GetItemAsync<string>(_accessTokenKey);
    }

    /// <summary>
    /// Получить токен перевыпуска.
    /// </summary>
    /// <returns>Токен перевыпуска.</returns>
    public async Task<string?> GetRefreshTokenAsync()
    {
        return await _localStorage.GetItemAsync<string>(_refreshTokenKey);
    }

    /// <summary>
    /// Получить токены.
    /// </summary>
    /// <returns>Токены.</returns>
    public async Task<(string? AccessToken, string? RefreshToken)> GetTokens()
    {
        var accessToken = await GetAccessTokenAsync();
        var refreshToken = await GetRefreshTokenAsync();
        return (accessToken, refreshToken);
    }

    /// <summary>
    /// Очистить токены.
    /// </summary>
    public async Task ClearTokensAsync()
    {
        await _localStorage.RemoveItemAsync(_accessTokenKey);
        await _localStorage.RemoveItemAsync(_refreshTokenKey);
    }
}