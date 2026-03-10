using System.Reflection;
using LimonikOne.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LimonikOne.Shared.Infrastructure.Modules;

public static class ModuleExtensions
{
    public static IServiceCollection AddModules(
        this IServiceCollection services,
        IConfiguration configuration,
        IEnumerable<Assembly> moduleAssemblies
    )
    {
        var modules = DiscoverModules(moduleAssemblies);

        foreach (var module in modules)
        {
            module.Register(services, configuration);
        }

        services.AddSingleton<IReadOnlyList<IModule>>(modules);

        return services;
    }

    public static IApplicationBuilder UseModules(this IApplicationBuilder app)
    {
        var modules = app.ApplicationServices.GetRequiredService<IReadOnlyList<IModule>>();

        foreach (var module in modules)
        {
            module.Use(app);
        }

        return app;
    }

    private static List<IModule> DiscoverModules(IEnumerable<Assembly> assemblies)
    {
        return assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t =>
                typeof(IModule).IsAssignableFrom(t)
                && t is { IsInterface: false, IsAbstract: false }
            )
            .Select(Activator.CreateInstance)
            .Cast<IModule>()
            .ToList();
    }
}
