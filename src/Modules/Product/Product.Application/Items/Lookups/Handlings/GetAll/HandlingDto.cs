namespace LimonikOne.Modules.Product.Application.Items.Lookups.Handlings.GetAll;

public sealed record HandlingDto(
    int Id,
    string Name,
    string Label,
    string? ShortName,
    string? Description
);
