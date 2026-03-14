using FluentValidation;
using LimonikOne.Modules.Scale.Api.Filters;
using LimonikOne.Modules.Scale.Application;
using LimonikOne.Modules.Scale.Application.WeightBatches.Ingest;
using LimonikOne.Modules.Scale.Domain.WeightBatches;
using LimonikOne.Modules.Scale.Domain.WeightEvents;
using LimonikOne.Modules.Scale.Domain.WeightReadings;
using LimonikOne.Modules.Scale.Infrastructure.Database;
using LimonikOne.Modules.Scale.Infrastructure.Repositories.WeightBatches;
using LimonikOne.Modules.Scale.Infrastructure.Repositories.WeightEvents;
using LimonikOne.Modules.Scale.Infrastructure.Repositories.WeightReadings;
using LimonikOne.Shared.Abstractions.Application;
using LimonikOne.Shared.Abstractions.Modules;
using LimonikOne.Shared.Infrastructure.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LimonikOne.Modules.Scale.Api;

public sealed class ScaleModule : IModule
{
    public string Name => "Scale";

    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        var postgresOptions = configuration
            .GetSection(PostgresOptions.SectionName)
            .Get<PostgresOptions>();

        if (postgresOptions is null || string.IsNullOrWhiteSpace(postgresOptions.ConnectionString))
        {
            throw new InvalidOperationException(
                $"Database configuration is missing. Set '{PostgresOptions.SectionName}:ConnectionString' in appsettings (e.g. appsettings.Development.json)."
            );
        }

        services.AddDbContext<ScaleDbContext>(options =>
        {
            options.UseNpgsql(postgresOptions.ConnectionString);
        });

        services
            .AddHealthChecks()
            .AddDbContextCheck<ScaleDbContext>(
                name: "postgres",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "db", "ready" }
            );

        // Unit of Work
        services.AddScoped<IScaleUnitOfWork, ScaleUnitOfWork>();

        // Weight Batch ingestion
        services.AddScoped<IWeightBatchRepository, WeightBatchRepository>();
        services.AddScoped<IWeightReadingRepository, WeightReadingRepository>();
        services.AddScoped<ICommandHandler<IngestWeightBatchCommand>, IngestWeightBatchHandler>();
        services.AddScoped<IValidator<IngestWeightBatchCommand>, IngestWeightBatchValidator>();

        // Weight Events
        services.AddScoped<IWeightEventRepository, WeightEventRepository>();

        services.AddScoped<ApiKeyAuthFilter>();
    }

    public void Use(IApplicationBuilder app)
    {
        // Module-specific middleware can be added here
    }
}
