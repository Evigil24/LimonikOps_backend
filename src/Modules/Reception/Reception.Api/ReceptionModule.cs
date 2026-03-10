using FluentValidation;
using LimonikOne.Modules.Reception.Api.Filters;
using LimonikOne.Modules.Reception.Application.Weights.Ingest;
using LimonikOne.Modules.Reception.Domain.Weights;
using LimonikOne.Modules.Reception.Infrastructure.Database;
using LimonikOne.Modules.Reception.Infrastructure.Repositories;
using LimonikOne.Shared.Abstractions.Application;
using LimonikOne.Shared.Abstractions.Modules;
using LimonikOne.Shared.Infrastructure.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LimonikOne.Modules.Reception.Api;

public sealed class ReceptionModule : IModule
{
    public string Name => "Reception";

    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        var postgresOptions = configuration
            .GetSection(PostgresOptions.SectionName)
            .Get<PostgresOptions>();

        if (postgresOptions is null || string.IsNullOrWhiteSpace(postgresOptions.ConnectionString))
        {
            throw new InvalidOperationException(
                $"Database configuration is missing. Set '{PostgresOptions.SectionName}:ConnectionString' in appsettings (e.g. appsettings.Development.json).");
        }

        services.AddDbContext<ReceptionDbContext>(options =>
        {
            options.UseNpgsql(postgresOptions.ConnectionString);
        });

        services
            .AddHealthChecks()
            .AddDbContextCheck<ReceptionDbContext>(
                name: "postgres",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "db", "ready" });

        // Weight Batch ingestion
        services.AddScoped<IWeightBatchRepository, WeightBatchRepository>();
        services.AddScoped<ICommandHandler<IngestWeightBatchCommand>, IngestWeightBatchHandler>();
        services.AddScoped<IValidator<IngestWeightBatchCommand>, IngestWeightBatchValidator>();
        services.AddScoped<ApiKeyAuthFilter>();
    }

    public void Use(IApplicationBuilder app)
    {
        // Module-specific middleware can be added here
    }
}
