using FluentAssertions;
using NetArchTest.Rules;

namespace LimonikOne.Tests.Architecture;

public class CrossModuleBoundaryTests
{
    [Theory]
    [InlineData("LimonikOne.Modules.Print")]
    public void Scale_Domain_Should_Not_Reference_Other_Modules(string otherModule)
    {
        var result = Types
            .InAssembly(
                typeof(LimonikOne.Modules.Scale.Domain.WeightBatches.WeightBatchId).Assembly
            )
            .ShouldNot()
            .HaveDependencyOn(otherModule)
            .GetResult();

        result.IsSuccessful.Should().BeTrue("Scale.Domain should not reference {0}", otherModule);
    }

    [Theory]
    [InlineData("LimonikOne.Modules.Print")]
    public void Scale_Application_Should_Not_Reference_Other_Modules(string otherModule)
    {
        var result = Types
            .InAssembly(
                typeof(LimonikOne.Modules.Scale.Application.WeightBatches.Ingest.IngestWeightBatchCommand).Assembly
            )
            .ShouldNot()
            .HaveDependencyOn(otherModule)
            .GetResult();

        result
            .IsSuccessful.Should()
            .BeTrue("Scale.Application should not reference {0}", otherModule);
    }

    [Theory]
    [InlineData("LimonikOne.Modules.Scale")]
    public void Print_Domain_Should_Not_Reference_Other_Modules(string otherModule)
    {
        var result = Types
            .InAssembly(typeof(LimonikOne.Modules.Print.Domain.PrintJobs.PrintJobId).Assembly)
            .ShouldNot()
            .HaveDependencyOn(otherModule)
            .GetResult();

        result.IsSuccessful.Should().BeTrue("Print.Domain should not reference {0}", otherModule);
    }

    [Theory]
    [InlineData("LimonikOne.Modules.Scale")]
    public void Print_Application_Should_Not_Reference_Other_Modules(string otherModule)
    {
        var result = Types
            .InAssembly(
                typeof(LimonikOne.Modules.Print.Application.PrintJobs.Enqueue.EnqueuePrintJobCommand).Assembly
            )
            .ShouldNot()
            .HaveDependencyOn(otherModule)
            .GetResult();

        result
            .IsSuccessful.Should()
            .BeTrue("Print.Application should not reference {0}", otherModule);
    }
}
