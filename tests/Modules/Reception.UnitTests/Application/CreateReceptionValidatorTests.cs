using FluentAssertions;
using LimonikOne.Modules.Reception.Application.Receptions.Create;

namespace LimonikOne.Modules.Reception.UnitTests.Application;

public class CreateReceptionValidatorTests
{
    private readonly CreateReceptionValidator _validator = new();

    [Fact]
    public void Should_Pass_With_Valid_Command()
    {
        var command = new CreateReceptionCommand("John", "Doe", "Some notes");

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_FirstName_Is_Empty()
    {
        var command = new CreateReceptionCommand("", "Doe", null);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "FirstName");
    }

    [Fact]
    public void Should_Fail_When_LastName_Is_Empty()
    {
        var command = new CreateReceptionCommand("John", "", null);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "LastName");
    }

    [Fact]
    public void Should_Fail_When_FirstName_Exceeds_MaxLength()
    {
        var command = new CreateReceptionCommand(new string('A', 101), "Doe", null);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Fail_When_Notes_Exceed_MaxLength()
    {
        var command = new CreateReceptionCommand("John", "Doe", new string('A', 501));

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }
}
