using FluentAssertions;
using LimonikOne.Modules.Reception.Domain.Receptions;

namespace LimonikOne.Modules.Reception.UnitTests.Domain;

public class GuestNameTests
{
    [Fact]
    public void Create_Should_Set_FirstName_And_LastName()
    {
        var guestName = GuestName.Create("John", "Doe");

        guestName.FirstName.Should().Be("John");
        guestName.LastName.Should().Be("Doe");
    }

    [Fact]
    public void Create_Should_Trim_Whitespace()
    {
        var guestName = GuestName.Create("  John  ", "  Doe  ");

        guestName.FirstName.Should().Be("John");
        guestName.LastName.Should().Be("Doe");
    }

    [Fact]
    public void FullName_Should_Return_FirstName_And_LastName()
    {
        var guestName = GuestName.Create("John", "Doe");

        guestName.FullName.Should().Be("John Doe");
    }

    [Theory]
    [InlineData("", "Doe")]
    [InlineData("  ", "Doe")]
    [InlineData(null, "Doe")]
    public void Create_Should_Throw_When_FirstName_Is_Invalid(string? firstName, string lastName)
    {
        var act = () => GuestName.Create(firstName!, lastName);

        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("John", "")]
    [InlineData("John", "  ")]
    [InlineData("John", null)]
    public void Create_Should_Throw_When_LastName_Is_Invalid(string firstName, string? lastName)
    {
        var act = () => GuestName.Create(firstName, lastName!);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Equals_Should_Return_True_For_Same_Values()
    {
        var name1 = GuestName.Create("John", "Doe");
        var name2 = GuestName.Create("John", "Doe");

        name1.Should().Be(name2);
    }

    [Fact]
    public void Equals_Should_Return_False_For_Different_Values()
    {
        var name1 = GuestName.Create("John", "Doe");
        var name2 = GuestName.Create("Jane", "Doe");

        name1.Should().NotBe(name2);
    }
}
