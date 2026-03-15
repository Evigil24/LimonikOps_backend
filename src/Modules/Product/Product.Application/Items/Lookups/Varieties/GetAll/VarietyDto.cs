namespace LimonikOne.Modules.Product.Application.Items.Lookups.Varieties.GetAll;

public sealed record VarietyDto(
    int Id,
    string Name,
    string Label,
    string? ShortName,
    string? Description
);
