using LimonikOne.Shared.Abstractions.Domain;

namespace LimonikOne.Modules.Product.Domain.Products;

public sealed class Product : AggregateRoot<ProductId>
{
    public string ItemNumber { get; private set; } = null!;
    public string PrimaryName { get; private set; } = null!;
    public string SearchName { get; private set; } = null!;
    public string Variety { get; private set; } = null!;
    public string Handling { get; private set; } = null!;
    public string Certification { get; private set; } = null!;
    public string Stage { get; private set; } = null!;

    private Product() { } // EF Core

    public static Product Create(
        string itemNumber,
        string primaryName,
        string searchName,
        string variety,
        string handling,
        string certification,
        string stage
    )
    {
        return new Product
        {
            Id = ProductId.New(),
            ItemNumber = itemNumber,
            PrimaryName = primaryName,
            SearchName = searchName,
            Variety = variety,
            Handling = handling,
            Certification = certification,
            Stage = stage,
        };
    }
}
