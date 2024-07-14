namespace Shared;

/// <summary>
/// Параметры конфигурации.
/// </summary>
public class ConfigurationOptions
{
    /// <summary>
    /// Путь к серверу аутентификации.
    /// </summary>
    public string AuthenticationServerUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// Путь к клиенту аутентификации.
    /// </summary>
    public string AuthenticationClientUrl { get; set; } = string.Empty;
}
