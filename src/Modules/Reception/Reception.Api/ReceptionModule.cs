using FluentValidation;
using LimonikOne.Modules.Reception.Api.Filters;
using LimonikOne.Modules.Reception.Application.Receptions.Create;
using LimonikOne.Modules.Reception.Application.Receptions.EventHandlers;
using LimonikOne.Modules.Reception.Application.Receptions.Get;
using LimonikOne.Modules.Reception.Application.Weights.Ingest;
using LimonikOne.Modules.Reception.Domain.Receptions;
using LimonikOne.Modules.Reception.Domain.Receptions.Events;
using LimonikOne.Modules.Reception.Domain.Weights;
using LimonikOne.Modules.Reception.Infrastructure.Database;
using LimonikOne.Modules.Reception.Infrastructure.Repositories;
using LimonikOne.Shared.Abstractions.Application;
using LimonikOne.Shared.Abstractions.Domain;
using LimonikOne.Shared.Abstractions.Modules;
using LimonikOne.Shared.Infrastructure.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LimonikOne.Modules.Reception.Api;

public sealed class ReceptionModule : IModule
{
    public string Name => "Reception";

    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        var postgresOptions = configuration.GetSection(PostgresOptions.SectionName).Get<PostgresOptions>();

        services.AddDbContext<ReceptionDbContext>(options =>
        {
            options.UseNpgsql(postgresOptions!.ConnectionString);
        });

        // Repositories
        services.AddScoped<IReceptionRepository, ReceptionRepository>();

        // Command Handlers
        services.AddScoped<ICommandHandler<CreateReceptionCommand, Guid>, CreateReceptionHandler>();

        // Query Handlers
        services.AddScoped<IQueryHandler<GetReceptionByIdQuery, ReceptionDto>, GetReceptionByIdHandler>();

        // Validators
        services.AddScoped<IValidator<CreateReceptionCommand>, CreateReceptionValidator>();

        // Domain Event Handlers
        services.AddScoped<IDomainEventHandler<ReceptionCreatedEvent>, ReceptionCreatedEventHandler>();

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
