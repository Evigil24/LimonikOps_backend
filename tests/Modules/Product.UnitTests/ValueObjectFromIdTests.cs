using FluentAssertions;
using LimonikOne.Modules.Product.Domain.Products;

namespace LimonikOne.Modules.Product.UnitTests;

public class VarietyFromIdTests
{
    [Fact]
    public void FromId_With_Valid_Id_Returns_Correct_Variety()
    {
        Variety.FromId(1).Should().Be(Variety.Persian);
        Variety.FromId(2).Should().Be(Variety.Mexican);
    }

    [Fact]
    public void FromId_With_Invalid_Id_Throws()
    {
        var act = () => Variety.FromId(999);
        act.Should().Throw<InvalidOperationException>().WithMessage("*999*");
    }
}

public class HandlingFromIdTests
{
    [Fact]
    public void FromId_With_Valid_Id_Returns_Correct_Handling()
    {
        Handling.FromId(1).Should().Be(Handling.Conventional);
        Handling.FromId(2).Should().Be(Handling.Organic);
    }

    [Fact]
    public void FromId_With_Invalid_Id_Throws()
    {
        var act = () => Handling.FromId(999);
        act.Should().Throw<InvalidOperationException>().WithMessage("*999*");
    }
}

public class CertificationFromIdTests
{
    [Fact]
    public void FromId_With_Valid_Id_Returns_Correct_Certification()
    {
        Certification.FromId(1).Should().Be(Certification.Without);
        Certification.FromId(2).Should().Be(Certification.FairTrade);
    }

    [Fact]
    public void FromId_With_Invalid_Id_Throws()
    {
        var act = () => Certification.FromId(999);
        act.Should().Throw<InvalidOperationException>().WithMessage("*999*");
    }
}

public class StageFromIdTests
{
    [Fact]
    public void FromId_With_Valid_Id_Returns_Correct_Stage()
    {
        Stage.FromId(1).Should().Be(Stage.Bulk);
        Stage.FromId(2).Should().Be(Stage.Sorted);
        Stage.FromId(3).Should().Be(Stage.Finished);
        Stage.FromId(4).Should().Be(Stage.Byproduct);
    }

    [Fact]
    public void FromId_With_Invalid_Id_Throws()
    {
        var act = () => Stage.FromId(999);
        act.Should().Throw<InvalidOperationException>().WithMessage("*999*");
    }
}
