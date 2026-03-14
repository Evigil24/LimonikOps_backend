using FluentValidation;
using LimonikOne.Modules.Print.Api.Filters;
using LimonikOne.Modules.Print.Application.PrintJobs.Claim;
using LimonikOne.Modules.Print.Application.PrintJobs.Complete;
using LimonikOne.Modules.Print.Application.PrintJobs.Enqueue;
using LimonikOne.Modules.Print.Application.PrintJobs.Fail;
using LimonikOne.Modules.Print.Domain.PrintJobs;
using LimonikOne.Modules.Print.Infrastructure.Database;
using LimonikOne.Modules.Print.Infrastructure.Repositories;
using LimonikOne.Shared.Abstractions.Application;
using LimonikOne.Shared.Abstractions.Modules;
using LimonikOne.Shared.Infrastructure.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LimonikOne.Modules.Print.Api;

public sealed class PrintModule : IModule
{
    public string Name => "Print";

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

        services.AddDbContext<PrintDbContext>(options =>
        {
            options.UseNpgsql(postgresOptions.ConnectionString);
        });

        services
            .AddHealthChecks()
            .AddDbContextCheck<PrintDbContext>(
                name: "print-db",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "db", "ready" }
            );

        // Repository
        services.AddScoped<IPrintJobRepository, PrintJobRepository>();

        // Enqueue
        services.AddScoped<ICommandHandler<EnqueuePrintJobCommand, Guid>, EnqueuePrintJobHandler>();
        services.AddScoped<IValidator<EnqueuePrintJobCommand>, EnqueuePrintJobValidator>();

        // Claim
        services.AddScoped<
            ICommandHandler<ClaimPrintJobCommand, ClaimPrintJobResult?>,
            ClaimPrintJobHandler
        >();
        services.AddScoped<IValidator<ClaimPrintJobCommand>, ClaimPrintJobValidator>();

        // Complete
        services.AddScoped<ICommandHandler<CompletePrintJobCommand>, CompletePrintJobHandler>();
        services.AddScoped<IValidator<CompletePrintJobCommand>, CompletePrintJobValidator>();

        // Fail
        services.AddScoped<ICommandHandler<FailPrintJobCommand>, FailPrintJobHandler>();
        services.AddScoped<IValidator<FailPrintJobCommand>, FailPrintJobValidator>();

        // Auth filter
        services.AddScoped<PrintApiKeyAuthFilter>();
    }

    public void Use(IApplicationBuilder app) { }
}
