using LimonikOne.Shared.Abstractions.Dynamics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LimonikOne.Shared.Infrastructure.Dynamics;

public static class Extensions
{
    public static IServiceCollection AddDynamics(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<DynamicsOptions>(configuration.GetSection(DynamicsOptions.SectionName));

        services.AddHttpClient("Dynamics.Token");

        services.AddTransient<DynamicsAuthHandler>();

        services
            .AddHttpClient<IDynamicsHttpClient, DynamicsHttpClient>()
            .AddHttpMessageHandler<DynamicsAuthHandler>()
            .AddStandardResilienceHandler();

        return services;
    }
}
