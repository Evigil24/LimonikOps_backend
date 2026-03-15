using FluentValidation;
using LimonikOne.Modules.Product.Application;
using LimonikOne.Modules.Product.Application.Items;
using LimonikOne.Modules.Product.Application.Items.Create;
using LimonikOne.Modules.Product.Application.Items.GetAll;
using LimonikOne.Modules.Product.Application.Items.GetById;
using LimonikOne.Modules.Product.Application.Items.Lookups.Certifications.GetAll;
using LimonikOne.Modules.Product.Application.Items.Lookups.Handlings.GetAll;
using LimonikOne.Modules.Product.Application.Items.Lookups.Stages.GetAll;
using LimonikOne.Modules.Product.Application.Items.Lookups.Varieties.GetAll;
using LimonikOne.Modules.Product.Domain.Items;
using LimonikOne.Modules.Product.Infrastructure.Database;
using LimonikOne.Modules.Product.Infrastructure.Repositories.Items;
using LimonikOne.Shared.Abstractions.Application;
using LimonikOne.Shared.Abstractions.Modules;
using LimonikOne.Shared.Infrastructure.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LimonikOne.Modules.Product.Api;

public sealed class ProductModule : IModule
{
    public string Name => "Product";

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

        services.AddDbContext<ProductDbContext>(options =>
        {
            options.UseNpgsql(postgresOptions.ConnectionString);
        });

        services
            .AddHealthChecks()
            .AddDbContextCheck<ProductDbContext>(
                name: "product-db",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "db", "ready" }
            );

        // Unit of Work
        services.AddScoped<IProductUnitOfWork, ProductUnitOfWork>();

        // Items
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<ICommandHandler<CreateItemCommand, Guid>, CreateItemHandler>();
        services.AddScoped<IValidator<CreateItemCommand>, CreateItemValidator>();
        services.AddScoped<
            IQueryHandler<GetAllItemsQuery, IReadOnlyList<ItemDto>>,
            GetAllItemsHandler
        >();
        services.AddScoped<IQueryHandler<GetItemByIdQuery, ItemDto>, GetItemByIdHandler>();

        // Queries (singleton: handlers return static domain lists, no scoped deps)
        services.AddSingleton<
            IQueryHandler<GetAllStagesQuery, IReadOnlyList<StageDto>>,
            GetAllStagesHandler
        >();
        services.AddSingleton<
            IQueryHandler<GetAllCertificationsQuery, IReadOnlyList<CertificationDto>>,
            GetAllCertificationsHandler
        >();
        services.AddSingleton<
            IQueryHandler<GetAllHandlingsQuery, IReadOnlyList<HandlingDto>>,
            GetAllHandlingsHandler
        >();
        services.AddSingleton<
            IQueryHandler<GetAllVarietiesQuery, IReadOnlyList<VarietyDto>>,
            GetAllVarietiesHandler
        >();
    }

    public void Use(IApplicationBuilder app) { }
}
