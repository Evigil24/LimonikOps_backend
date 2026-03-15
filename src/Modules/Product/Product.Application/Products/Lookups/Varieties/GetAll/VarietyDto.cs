namespace LimonikOne.Modules.Product.Application.Products.Lookups.Varieties.GetAll;

public sealed record VarietyDto(
    int Id,
    string Name,
    string Label,
    string? ShortName,
    string? Description
);
