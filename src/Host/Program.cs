using LimonikOne.Modules.Print.Api;
using LimonikOne.Modules.Scale.Api;
using LimonikOne.Shared.Infrastructure.Extensions;
using LimonikOne.Shared.Infrastructure.Modules;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog(
    (context, configuration) =>
    {
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console();
    }
);

// Add services
builder
    .Services.AddControllers()
    .AddApplicationPart(typeof(ScaleModule).Assembly)
    .AddApplicationPart(typeof(PrintModule).Assembly);

builder.Services.AddOpenApi();

builder.Services.AddSharedInfrastructure();

// Discover and register modules
var moduleAssemblies = new[] { typeof(ScaleModule).Assembly, typeof(PrintModule).Assembly };
builder.Services.AddModules(builder.Configuration, moduleAssemblies);

var app = builder.Build();

// Configure pipeline
app.UseSharedInfrastructure();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseSerilogRequestLogging();
app.MapControllers();
app.MapHealthChecks("/health");
app.UseModules();

app.Run();

// Make the implicit Program class public for integration tests
public partial class Program;
