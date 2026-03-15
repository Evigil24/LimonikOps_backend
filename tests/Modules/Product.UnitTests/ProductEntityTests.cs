using FluentAssertions;
using LimonikOne.Modules.Product.Domain.Products;
using ProductAggregate = LimonikOne.Modules.Product.Domain.Products.Product;

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

        var product = ProductAggregate.Create(
            itemNumber,
            primaryName,
            searchName,
            variety,
            handling,
            certification,
            stage
        );

        product.Should().NotBeNull();
        product.Id.Value.Should().NotBeEmpty();
        product.ItemNumber.Should().Be(itemNumber);
        product.PrimaryName.Should().Be(primaryName);
        product.SearchName.Should().Be(searchName);
        product.Variety.Should().Be(variety);
        product.Handling.Should().Be(handling);
        product.Certification.Should().Be(certification);
        product.Stage.Should().Be(stage);
    }
}
