namespace LimonikOne.Modules.Product.Application.Products.Lookups.Stages.GetAll;

public sealed record StageDto(
    int Id,
    string Name,
    string Label,
    string? ShortName,
    string? Description
);
