using ProductEntity = LimonikOne.Modules.Product.Domain.Products.Product;

namespace LimonikOne.Modules.Product.Application.Products;

public sealed record ProductDto(
    Guid Id,
    long DisplayId,
    string ItemNumber,
    string PrimaryName,
    string SearchName,
    string Variety,
    string Handling,
    string Certification,
    string Stage
)
{
    public static ProductDto FromEntity(ProductEntity product)
    {
        return new ProductDto(
            product.Id.Value,
            product.DisplayId,
            product.ItemNumber,
            product.PrimaryName,
            product.SearchName,
            product.Variety.Label,
            product.Handling.Label,
            product.Certification.Label,
            product.Stage.Label
        );
    }
}
