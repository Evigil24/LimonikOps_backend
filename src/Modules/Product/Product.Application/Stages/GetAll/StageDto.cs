namespace LimonikOne.Modules.Product.Application.Stages.GetAll;

public sealed record StageDto(
    int Id,
    string Name,
    string Label,
    string? ShortName,
    string? Description
);
