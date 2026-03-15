namespace LimonikOne.Modules.Product.Application.Items.Lookups.Stages.GetAll;

public sealed record StageDto(
    int Id,
    string Name,
    string Label,
    string? ShortName,
    string? Description
);
