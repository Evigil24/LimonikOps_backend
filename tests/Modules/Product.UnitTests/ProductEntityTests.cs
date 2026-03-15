using FluentAssertions;
using LimonikOne.Modules.Product.Domain.Items;

namespace LimonikOne.Modules.Product.UnitTests;

public class ProductEntityTests
{
    [Fact]
    public void Create_Produces_Valid_Aggregate_With_Correct_Properties()
    {
        var variety = Variety.Persian;
        var handling = Handling.Conventional;
        var certification = Certification.Without;
        var stage = Stage.Bulk;
        var itemNumber = "ITEM-001";
        var primaryName = "Lemon Bulk";
        var searchName = "lemon bulk conventional";

        var item = Item.Create(
            itemNumber,
            primaryName,
            searchName,
            variety,
            handling,
            certification,
            stage
        );

        item.Should().NotBeNull();
        item.Id.Value.Should().NotBeEmpty();
        item.ItemNumber.Should().Be(itemNumber);
        item.PrimaryName.Should().Be(primaryName);
        item.SearchName.Should().Be(searchName);
        item.Variety.Should().Be(variety);
        item.Handling.Should().Be(handling);
        item.Certification.Should().Be(certification);
        item.Stage.Should().Be(stage);
    }
}
