using LimonikOne.Modules.Person.Application;
using LimonikOne.Modules.Person.Application.Customers;
using LimonikOne.Modules.Person.Application.Customers.GetAll;
using LimonikOne.Modules.Person.Application.Customers.GetById;
using LimonikOne.Modules.Person.Application.Customers.Refresh;
using LimonikOne.Modules.Person.Application.VendorClassifications;
using LimonikOne.Modules.Person.Application.VendorClassifications.GetAll;
using LimonikOne.Modules.Person.Application.VendorClassifications.GetById;
using LimonikOne.Modules.Person.Application.Vendors;
using LimonikOne.Modules.Person.Application.Vendors.GetAll;
using LimonikOne.Modules.Person.Application.Vendors.GetById;
using LimonikOne.Modules.Person.Application.Vendors.Refresh;
using LimonikOne.Modules.Person.Domain.Customers;
using LimonikOne.Modules.Person.Domain.VendorClassifications;
using LimonikOne.Modules.Person.Domain.Vendors;
using LimonikOne.Modules.Person.Infrastructure.Database;
using LimonikOne.Modules.Person.Infrastructure.Repositories.Customers;
using LimonikOne.Modules.Person.Infrastructure.Repositories.VendorClassifications;
using LimonikOne.Modules.Person.Infrastructure.Repositories.Vendors;
using LimonikOne.Shared.Abstractions.Application;
using LimonikOne.Shared.Abstractions.Modules;
using LimonikOne.Shared.Infrastructure.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LimonikOne.Modules.Person.Api;

public sealed class PersonModule : IModule
{
    public string Name => "Person";

    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        var postgresOptions = configuration
            .GetSection(PostgresOptions.SectionName)
            .Get<PostgresOptions>();

        if (postgresOptions is null || string.IsNullOrWhiteSpace(postgresOptions.ConnectionString))
        {
            throw new InvalidOperationException(
                "Database configuration is missing. Ensure 'Postgres:ConnectionString' is set."
            );
        }

        services.AddDbContext<PersonDbContext>(options =>
        {
            options.UseNpgsql(postgresOptions.ConnectionString);
        });

        services
            .AddHealthChecks()
            .AddDbContextCheck<PersonDbContext>(
                name: "person-db",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "db", "ready" }
            );

        // Unit of Work
        services.AddScoped<IPersonUnitOfWork, PersonUnitOfWork>();

        // Repositories
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IVendorRepository, VendorRepository>();
        services.AddScoped<IVendorClassificationRepository, VendorClassificationRepository>();

        // Command Handlers
        services.AddScoped<ICommandHandler<RefreshCustomersCommand>, RefreshCustomersHandler>();
        services.AddScoped<ICommandHandler<RefreshVendorsCommand>, RefreshVendorsHandler>();

        // Query Handlers
        services.AddScoped<
            IQueryHandler<GetAllCustomersQuery, IReadOnlyList<CustomerDto>>,
            GetAllCustomersHandler
        >();
        services.AddScoped<
            IQueryHandler<GetCustomerByIdQuery, CustomerDto>,
            GetCustomerByIdHandler
        >();
        services.AddScoped<
            IQueryHandler<GetAllVendorsQuery, IReadOnlyList<VendorDto>>,
            GetAllVendorsHandler
        >();
        services.AddScoped<IQueryHandler<GetVendorByIdQuery, VendorDto>, GetVendorByIdHandler>();
        services.AddScoped<
            IQueryHandler<GetAllVendorClassificationsQuery, IReadOnlyList<VendorClassificationDto>>,
            GetAllVendorClassificationsHandler
        >();
        services.AddScoped<
            IQueryHandler<GetVendorClassificationByIdQuery, VendorClassificationDto>,
            GetVendorClassificationByIdHandler
        >();
    }

    public void Use(IApplicationBuilder app) { }
}
