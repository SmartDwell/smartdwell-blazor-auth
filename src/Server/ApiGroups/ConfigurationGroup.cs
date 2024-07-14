using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Shared;

namespace Server.ApiGroups;

/// <summary>
/// Группа настроек.
/// </summary>
public static class ConfigurationGroup
{
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
    }
    
    private static Ok<ConfigurationOptions> GetConfigurations(IOptions<ConfigurationOptions> configurationOptions)
    {
        return TypedResults.Ok(configurationOptions.Value);
    }
}