using Client.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Shared;

namespace Server.ApiGroups;

/// <summary>
/// Группа настроек.
/// </summary>
public static class ConfigurationGroup
{
    private const int TokenLifetime = 60 * 60 * 24 * 7; // 7 days
    private const string AccessTokenCookieName = "access_token";
    private const string RefreshTokenCookieName = "refresh_token";
    
    /// <summary>
    /// Конфигурация группы.
    /// </summary>
    /// <param name="endpoints">Маршруты.</param>
    public static void MapConfigurationGroup(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup(RouteConstants.ConfigurationData.Route);
        group.MapGet(RouteConstants.ConfigurationData.Options, GetConfigurations)
            .WithName("GetConfigurations")
            .WithSummary("Получение настроек.")
            .WithOpenApi();
        group.MapPost(RouteConstants.ConfigurationData.Tokens, SaveTokens)
            .WithName("SaveTokens")
            .WithSummary("Сохранение токенов.")
            .WithOpenApi();
        group.MapGet(RouteConstants.ConfigurationData.Tokens, GetTokens)
            .WithName("GetTokens")
            .WithSummary("Получение токенов.")
            .WithOpenApi();
    }
    
    private static Ok<ConfigurationOptions> GetConfigurations(IOptions<ConfigurationOptions> configurationOptions)
    {
        return TypedResults.Ok(configurationOptions.Value);
    }

    private static Ok SaveTokens(HttpContext context, TokensDto tokensDto)
    {
        var optionsLax = new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddMinutes(30)
        };
        context.Response.Cookies.Append(AccessTokenCookieName, tokensDto.AccessToken, optionsLax);
        context.Response.Cookies.Append(RefreshTokenCookieName, tokensDto.RefreshToken, optionsLax);
        return TypedResults.Ok();
    }
    
    private static IResult GetTokens(HttpContext context)
    {
        var accessToken = context.Request.Cookies[AccessTokenCookieName];
        var refreshToken = context.Request.Cookies[RefreshTokenCookieName];
        
        if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(refreshToken))
        {
            return TypedResults.Ok(new TokensDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }
        
        return TypedResults.NotFound();
    }
}