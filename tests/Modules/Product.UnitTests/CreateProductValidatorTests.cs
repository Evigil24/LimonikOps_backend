using FluentAssertions;
using LimonikOne.Modules.Product.Application.Items.Create;
using Xunit;

namespace LimonikOne.Modules.Product.UnitTests;

public class CreateItemValidatorTests
{
    private static CreateItemCommand ValidCommand() =>
        new(
            ItemNumber: "ITEM-001",
            PrimaryName: "Lemon Bulk",
            SearchName: "lemon bulk",
            VarietyId: 1,
            HandlingId: 1,
            CertificationId: 1,
            StageId: 1
        );

    private readonly CreateItemValidator _sut = new();

    [Fact]
    public void Valid_Input_Passes_Validation()
    {
        var result = _sut.Validate(ValidCommand());
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ItemNumber_Empty_Fails()
    {
        var cmd = ValidCommand() with { ItemNumber = "" };
        var result = _sut.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateItemCommand.ItemNumber));
    }

    [Fact]
    public void ItemNumber_Exceeds_MaxLength_Fails()
    {
        var cmd = ValidCommand() with { ItemNumber = new string('x', 101) };
        var result = _sut.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateItemCommand.ItemNumber));
    }

    [Fact]
    public void PrimaryName_Empty_Fails()
    {
        var cmd = ValidCommand() with { PrimaryName = "" };
        var result = _sut.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result
            .Errors.Should()
            .Contain(e => e.PropertyName == nameof(CreateItemCommand.PrimaryName));
    }

    [Fact]
    public void PrimaryName_Exceeds_MaxLength_Fails()
    {
        var cmd = ValidCommand() with { PrimaryName = new string('x', 301) };
        var result = _sut.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result
            .Errors.Should()
            .Contain(e => e.PropertyName == nameof(CreateItemCommand.PrimaryName));
    }

    [Fact]
    public void SearchName_Empty_Fails()
    {
        var cmd = ValidCommand() with { SearchName = "" };
        var result = _sut.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateItemCommand.SearchName));
    }

    [Fact]
    public void SearchName_Exceeds_MaxLength_Fails()
    {
        var cmd = ValidCommand() with { SearchName = new string('x', 501) };
        var result = _sut.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateItemCommand.SearchName));
    }

    [Fact]
    public void Invalid_VarietyId_Fails()
    {
        var cmd = ValidCommand() with { VarietyId = 999 };
        var result = _sut.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateItemCommand.VarietyId));
    }

    [Fact]
    public void Invalid_HandlingId_Fails()
    {
        var cmd = ValidCommand() with { HandlingId = 999 };
        var result = _sut.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateItemCommand.HandlingId));
    }

    [Fact]
    public void Invalid_CertificationId_Fails()
    {
        var cmd = ValidCommand() with { CertificationId = 999 };
        var result = _sut.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result
            .Errors.Should()
            .Contain(e => e.PropertyName == nameof(CreateItemCommand.CertificationId));
    }

    [Fact]
    public void Invalid_StageId_Fails()
    {
        var cmd = ValidCommand() with { StageId = 999 };
        var result = _sut.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateItemCommand.StageId));
    }
}
