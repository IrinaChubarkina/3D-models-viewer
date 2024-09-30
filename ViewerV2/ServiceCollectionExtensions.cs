using Viewer.Configuration;
using ViewerV2.Services;

namespace ViewerV2;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddTokenService(config);

        return services;
    }

    private static IServiceCollection AddTokenService(this IServiceCollection services, IConfiguration config)
    {
        var authConfig = config.GetSection("auth").Get<AuthConfig>()!;
        services.AddSingleton(authConfig ?? throw new ArgumentNullException(nameof(authConfig)));

        services.AddHttpClient(nameof(ForgeTokenService),
            client => { client.BaseAddress = new Uri(authConfig.ApiBaseAddress); });

        services.AddScoped<IForgeTokenService, ForgeTokenService>();
        return services;
    }
}