using FluentAssertions;
using NetArchTest.Rules;

namespace LimonikOne.Tests.Architecture;

public class ModuleArchitectureTests
{
    private const string DomainNamespace = "LimonikOne.Modules.Reception.Domain";
    private const string ApplicationNamespace = "LimonikOne.Modules.Reception.Application";
    private const string InfrastructureNamespace = "LimonikOne.Modules.Reception.Infrastructure";
    private const string ApiNamespace = "LimonikOne.Modules.Reception.Api";

    [Fact]
    public void Domain_Should_Not_Depend_On_Application()
    {
        var result = Types.InAssembly(typeof(LimonikOne.Modules.Reception.Domain.Receptions.ReceptionId).Assembly)
            .ShouldNot()
            .HaveDependencyOn(ApplicationNamespace)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            "Domain should not depend on Application. Failing types: {0}",
            result.FailingTypeNames is not null ? string.Join(", ", result.FailingTypeNames) : "none");
    }

    [Fact]
    public void Domain_Should_Not_Depend_On_Infrastructure()
    {
        var result = Types.InAssembly(typeof(LimonikOne.Modules.Reception.Domain.Receptions.ReceptionId).Assembly)
            .ShouldNot()
            .HaveDependencyOn(InfrastructureNamespace)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            "Domain should not depend on Infrastructure. Failing types: {0}",
            result.FailingTypeNames is not null ? string.Join(", ", result.FailingTypeNames) : "none");
    }

    [Fact]
    public void Application_Should_Not_Depend_On_Infrastructure()
    {
        var result = Types.InAssembly(typeof(LimonikOne.Modules.Reception.Application.Receptions.Create.CreateReceptionCommand).Assembly)
            .ShouldNot()
            .HaveDependencyOn(InfrastructureNamespace)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            "Application should not depend on Infrastructure. Failing types: {0}",
            result.FailingTypeNames is not null ? string.Join(", ", result.FailingTypeNames) : "none");
    }

    [Fact]
    public void Api_Should_Not_Depend_On_Infrastructure_Directly()
    {
        // Api references Infrastructure only for module registration (IModule implementation),
        // but controllers should not directly use Infrastructure types
        var result = Types.InAssembly(typeof(LimonikOne.Modules.Reception.Api.ReceptionModule).Assembly)
            .That()
            .ResideInNamespace("LimonikOne.Modules.Reception.Api.Controllers")
            .ShouldNot()
            .HaveDependencyOn(InfrastructureNamespace)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            "Api controllers should not depend on Infrastructure directly. Failing types: {0}",
            result.FailingTypeNames is not null ? string.Join(", ", result.FailingTypeNames) : "none");
    }
}
