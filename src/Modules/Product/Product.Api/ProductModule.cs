using FluentValidation;
using LimonikOne.Modules.Product.Application;
using LimonikOne.Modules.Product.Application.Products;
using LimonikOne.Modules.Product.Application.Products.Create;
using LimonikOne.Modules.Product.Application.Products.GetAll;
using LimonikOne.Modules.Product.Application.Products.GetById;
using LimonikOne.Modules.Product.Application.Products.Lookups.Certifications.GetAll;
using LimonikOne.Modules.Product.Application.Products.Lookups.Handlings.GetAll;
using LimonikOne.Modules.Product.Application.Products.Lookups.Stages.GetAll;
using LimonikOne.Modules.Product.Application.Products.Lookups.Varieties.GetAll;
using LimonikOne.Modules.Product.Domain.Products;
using LimonikOne.Modules.Product.Infrastructure.Database;
using LimonikOne.Modules.Product.Infrastructure.Repositories.Products;
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

        // Products
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICommandHandler<CreateProductCommand, Guid>, CreateProductHandler>();
        services.AddScoped<IValidator<CreateProductCommand>, CreateProductValidator>();
        services.AddScoped<
            IQueryHandler<GetAllProductsQuery, IReadOnlyList<ProductDto>>,
            GetAllProductsHandler
        >();
        services.AddScoped<IQueryHandler<GetProductByIdQuery, ProductDto>, GetProductByIdHandler>();

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
