using System.ComponentModel.DataAnnotations;

namespace Client.Shared;

/// <summary>
/// Модель данных для старта авторизации.
/// </summary>
public class AuthRequestCodeDto
{
    /// <summary>
    /// Логин пользователя.
    /// </summary>
    [Required]
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Тип логина при авторизации.
    /// </summary>
    [Required]
    public AuthLoginType AuthLoginType { get; set; } = AuthLoginType.Email;
    
    /// <summary>
    /// Информация об устройстве.
    /// </summary>
    public string? DeviceDescription { get; set; } = string.Empty;
}

/// <summary>
/// Тип логина при авторизации.
/// </summary>
public enum AuthLoginType
{
    /// <summary>
    /// Электронная почта.
    /// </summary>
    Email = 0,
}

/// <summary>
/// Модель данных для подтверждения авторизации.
/// </summary>
public class AuthVerifyCodeDto
{
    /// <summary>
    /// Тикет.
    /// </summary>
    [Required]
    public string TicketId { get; set; } = string.Empty;
    
    /// <summary>
    /// Код подтверждения.
    /// </summary>
    [Required]
    public string Code { get; set; } = string.Empty;
}

/// <summary>
/// Тикет для авторизации.
/// </summary>
public class TicketDto
{
    /// <summary>
    /// Наименование пользователя.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Идентификатор тикета.
    /// </summary>
    public string TicketId { get; set; } = string.Empty;
}

/// <summary>
/// Модель токенов.
/// </summary>
public class TokensDto
{
    /// <summary>
    /// Access-токен.
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;
    
    /// <summary>
    /// Refresh-токен.
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
}