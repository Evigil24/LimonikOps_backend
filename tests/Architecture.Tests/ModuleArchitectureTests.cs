using FluentAssertions;
using NetArchTest.Rules;

namespace LimonikOne.Tests.Architecture;

public class ModuleArchitectureTests
{
    private const string DomainNamespace = "LimonikOne.Modules.Scale.Domain";
    private const string ApplicationNamespace = "LimonikOne.Modules.Scale.Application";
    private const string InfrastructureNamespace = "LimonikOne.Modules.Scale.Infrastructure";
    private const string ApiNamespace = "LimonikOne.Modules.Scale.Api";

    [Fact]
    public void Domain_Should_Not_Depend_On_Application()
    {
        var result = Types
            .InAssembly(
                typeof(LimonikOne.Modules.Scale.Domain.WeightBatches.WeightBatchId).Assembly
            )
            .ShouldNot()
            .HaveDependencyOn(ApplicationNamespace)
            .GetResult();

        result
            .IsSuccessful.Should()
            .BeTrue(
                "Domain should not depend on Application. Failing types: {0}",
                result.FailingTypeNames is not null
                    ? string.Join(", ", result.FailingTypeNames)
                    : "none"
            );
    }

    [Fact]
    public void Domain_Should_Not_Depend_On_Infrastructure()
    {
        var result = Types
            .InAssembly(
                typeof(LimonikOne.Modules.Scale.Domain.WeightBatches.WeightBatchId).Assembly
            )
            .ShouldNot()
            .HaveDependencyOn(InfrastructureNamespace)
            .GetResult();

        result
            .IsSuccessful.Should()
            .BeTrue(
                "Domain should not depend on Infrastructure. Failing types: {0}",
                result.FailingTypeNames is not null
                    ? string.Join(", ", result.FailingTypeNames)
                    : "none"
            );
    }

    [Fact]
    public void Application_Should_Not_Depend_On_Infrastructure()
    {
        var result = Types
            .InAssembly(
                typeof(LimonikOne.Modules.Scale.Application.WeightBatches.Ingest.IngestWeightBatchCommand).Assembly
            )
            .ShouldNot()
            .HaveDependencyOn(InfrastructureNamespace)
            .GetResult();

        result
            .IsSuccessful.Should()
            .BeTrue(
                "Application should not depend on Infrastructure. Failing types: {0}",
                result.FailingTypeNames is not null
                    ? string.Join(", ", result.FailingTypeNames)
                    : "none"
            );
    }

    [Fact]
    public void Api_Should_Not_Depend_On_Infrastructure_Directly()
    {
        // Api references Infrastructure only for module registration (IModule implementation),
        // but controllers should not directly use Infrastructure types
        var result = Types
            .InAssembly(typeof(LimonikOne.Modules.Scale.Api.ScaleModule).Assembly)
            .That()
            .ResideInNamespace("LimonikOne.Modules.Scale.Api.Controllers")
            .ShouldNot()
            .HaveDependencyOn(InfrastructureNamespace)
            .GetResult();

        result
            .IsSuccessful.Should()
            .BeTrue(
                "Api controllers should not depend on Infrastructure directly. Failing types: {0}",
                result.FailingTypeNames is not null
                    ? string.Join(", ", result.FailingTypeNames)
                    : "none"
            );
    }

    // --- Print module architecture tests ---

    private const string PrintDomainNamespace = "LimonikOne.Modules.Print.Domain";
    private const string PrintApplicationNamespace = "LimonikOne.Modules.Print.Application";
    private const string PrintInfrastructureNamespace = "LimonikOne.Modules.Print.Infrastructure";

    [Fact]
    public void Print_Domain_Should_Not_Depend_On_Application()
    {
        var result = Types
            .InAssembly(typeof(LimonikOne.Modules.Print.Domain.PrintJobs.PrintJobId).Assembly)
            .ShouldNot()
            .HaveDependencyOn(PrintApplicationNamespace)
            .GetResult();

        result
            .IsSuccessful.Should()
            .BeTrue(
                "Print.Domain should not depend on Application. Failing types: {0}",
                result.FailingTypeNames is not null
                    ? string.Join(", ", result.FailingTypeNames)
                    : "none"
            );
    }

    [Fact]
    public void Print_Domain_Should_Not_Depend_On_Infrastructure()
    {
        var result = Types
            .InAssembly(typeof(LimonikOne.Modules.Print.Domain.PrintJobs.PrintJobId).Assembly)
            .ShouldNot()
            .HaveDependencyOn(PrintInfrastructureNamespace)
            .GetResult();

        result
            .IsSuccessful.Should()
            .BeTrue(
                "Print.Domain should not depend on Infrastructure. Failing types: {0}",
                result.FailingTypeNames is not null
                    ? string.Join(", ", result.FailingTypeNames)
                    : "none"
            );
    }

    [Fact]
    public void Print_Application_Should_Not_Depend_On_Infrastructure()
    {
        var result = Types
            .InAssembly(
                typeof(LimonikOne.Modules.Print.Application.PrintJobs.Enqueue.EnqueuePrintJobCommand).Assembly
            )
            .ShouldNot()
            .HaveDependencyOn(PrintInfrastructureNamespace)
            .GetResult();

        result
            .IsSuccessful.Should()
            .BeTrue(
                "Print.Application should not depend on Infrastructure. Failing types: {0}",
                result.FailingTypeNames is not null
                    ? string.Join(", ", result.FailingTypeNames)
                    : "none"
            );
    }

    [Fact]
    public void Print_Api_Should_Not_Depend_On_Infrastructure_Directly()
    {
        var result = Types
            .InAssembly(typeof(LimonikOne.Modules.Print.Api.PrintModule).Assembly)
            .That()
            .ResideInNamespace("LimonikOne.Modules.Print.Api.Controllers")
            .ShouldNot()
            .HaveDependencyOn(PrintInfrastructureNamespace)
            .GetResult();

        result
            .IsSuccessful.Should()
            .BeTrue(
                "Print.Api controllers should not depend on Infrastructure directly. Failing types: {0}",
                result.FailingTypeNames is not null
                    ? string.Join(", ", result.FailingTypeNames)
                    : "none"
            );
    }

    // --- Product module architecture tests ---

    private const string ProductDomainNamespace = "LimonikOne.Modules.Product.Domain";
    private const string ProductApplicationNamespace = "LimonikOne.Modules.Product.Application";
    private const string ProductInfrastructureNamespace =
        "LimonikOne.Modules.Product.Infrastructure";

    [Fact]
    public void Product_Domain_Should_Not_Depend_On_Application()
    {
        var result = Types
            .InAssembly(typeof(LimonikOne.Modules.Product.Domain.ProductDomainAssembly).Assembly)
            .ShouldNot()
            .HaveDependencyOn(ProductApplicationNamespace)
            .GetResult();

        result
            .IsSuccessful.Should()
            .BeTrue(
                "Product.Domain should not depend on Application. Failing types: {0}",
                result.FailingTypeNames is not null
                    ? string.Join(", ", result.FailingTypeNames)
                    : "none"
            );
    }

    [Fact]
    public void Product_Domain_Should_Not_Depend_On_Infrastructure()
    {
        var result = Types
            .InAssembly(typeof(LimonikOne.Modules.Product.Domain.ProductDomainAssembly).Assembly)
            .ShouldNot()
            .HaveDependencyOn(ProductInfrastructureNamespace)
            .GetResult();

        result
            .IsSuccessful.Should()
            .BeTrue(
                "Product.Domain should not depend on Infrastructure. Failing types: {0}",
                result.FailingTypeNames is not null
                    ? string.Join(", ", result.FailingTypeNames)
                    : "none"
            );
    }

    [Fact]
    public void Product_Application_Should_Not_Depend_On_Infrastructure()
    {
        var result = Types
            .InAssembly(
                typeof(LimonikOne.Modules.Product.Application.ProductApplicationAssembly).Assembly
            )
            .ShouldNot()
            .HaveDependencyOn(ProductInfrastructureNamespace)
            .GetResult();

        result
            .IsSuccessful.Should()
            .BeTrue(
                "Product.Application should not depend on Infrastructure. Failing types: {0}",
                result.FailingTypeNames is not null
                    ? string.Join(", ", result.FailingTypeNames)
                    : "none"
            );
    }

    [Fact]
    public void Product_Api_Should_Not_Depend_On_Infrastructure_Directly()
    {
        var result = Types
            .InAssembly(typeof(LimonikOne.Modules.Product.Api.ProductModule).Assembly)
            .That()
            .ResideInNamespace("LimonikOne.Modules.Product.Api.Controllers")
            .ShouldNot()
            .HaveDependencyOn(ProductInfrastructureNamespace)
            .GetResult();

        result
            .IsSuccessful.Should()
            .BeTrue(
                "Product.Api controllers should not depend on Infrastructure directly. Failing types: {0}",
                result.FailingTypeNames is not null
                    ? string.Join(", ", result.FailingTypeNames)
                    : "none"
            );
    }
}
