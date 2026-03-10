using FluentAssertions;
using NetArchTest.Rules;

namespace LimonikOne.Tests.Architecture;

public class CrossModuleBoundaryTests
{
    [Fact]
    public void Scale_Domain_Should_Not_Reference_Other_Modules()
    {
        // When more modules are added, add their namespaces here
        // For now, verify the pattern works with a self-referencing check
        var result = Types
            .InAssembly(typeof(LimonikOne.Modules.Scale.Domain.Weights.WeightBatchId).Assembly)
            .ShouldNot()
            .HaveDependencyOn("LimonikOne.Modules.Billing")
            .GetResult();

        result.IsSuccessful.Should().BeTrue("Scale.Domain should not reference other modules");
    }

    [Fact]
    public void Scale_Application_Should_Not_Reference_Other_Modules()
    {
        var result = Types
            .InAssembly(
                typeof(LimonikOne.Modules.Scale.Application.Weights.Ingest.IngestWeightBatchCommand).Assembly
            )
            .ShouldNot()
            .HaveDependencyOn("LimonikOne.Modules.Billing")
            .GetResult();

        result.IsSuccessful.Should().BeTrue("Scale.Application should not reference other modules");
    }
}
