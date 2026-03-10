using FluentAssertions;
using LimonikOne.Modules.Scale.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LimonikOne.Tests.Schema;

public class SchemaDriftTests
{
    private readonly string _connectionString;

    public SchemaDriftTests()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddUserSecrets(userSecretsId: "LimonikOne.Host-2025", reloadOnChange: false)
            .Build();

        _connectionString =
            configuration["Postgres:ConnectionString"]
            ?? throw new InvalidOperationException(
                "Postgres:ConnectionString not found in appsettings.json"
            );
    }

    public static TheoryData<string, string, Type> ModuleData() =>
        new()
        {
            // (ModuleName, SchemaName, DbContextType)
            { "Scale", "scale", typeof(ScaleDbContext) },
            // Add new modules here:
            // { "Purchase", "purchase", typeof(PurchaseDbContext) },
        };

    [Theory]
    [MemberData(nameof(ModuleData))]
    public async Task DatabaseSchema_ShouldMatch_EfCoreModel(
        string moduleName,
        string schemaName,
        Type dbContextType
    )
    {
        // Arrange: build DbContext to read the EF model
        var options = CreateDbContextOptions(dbContextType, _connectionString);
        await using var context = (DbContext)Activator.CreateInstance(dbContextType, options)!;

        // Act: read both schemas
        var efSchema = EfCoreModelReader.Read(context, schemaName);
        var dbSchema = await DatabaseSchemaReader.ReadAsync(_connectionString, schemaName);

        var differences = SchemaComparer.Compare(efSchema, dbSchema);

        // Assert
        if (differences.Count > 0)
        {
            var report = FormatReport(moduleName, differences);
            differences.Should().BeEmpty(report);
        }
    }

    private static object CreateDbContextOptions(Type dbContextType, string connectionString)
    {
        // Build DbContextOptions<TContext> using the generic method
        var optionsBuilderType = typeof(DbContextOptionsBuilder<>).MakeGenericType(dbContextType);
        var optionsBuilder = (DbContextOptionsBuilder)Activator.CreateInstance(optionsBuilderType)!;

        optionsBuilder.UseNpgsql(connectionString);

        return optionsBuilder.Options;
    }

    private static string FormatReport(string moduleName, List<SchemaDifference> differences)
    {
        var lines = new List<string>
        {
            $"Module '{moduleName}' has {differences.Count} schema difference(s):",
        };

        for (var i = 0; i < differences.Count; i++)
        {
            lines.Add($"  {i + 1}. {differences[i]}");
        }

        return string.Join(Environment.NewLine, lines);
    }
}
