using LimonikOne.Shared.Abstractions.Domain;
using LimonikOne.Shared.Infrastructure.DomainEvents;
using LimonikOne.Shared.Infrastructure.Exceptions;
using LimonikOne.Shared.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace LimonikOne.Shared.Infrastructure.Extensions;

public static class SharedInfrastructureExtensions
{
    public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventDispatcher, InProcessDomainEventDispatcher>();
        return services;
    }

    public static IApplicationBuilder UseSharedInfrastructure(this IApplicationBuilder app)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseMiddleware<ExceptionMiddleware>();
        return app;
    }
}
