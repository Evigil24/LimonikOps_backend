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
);
