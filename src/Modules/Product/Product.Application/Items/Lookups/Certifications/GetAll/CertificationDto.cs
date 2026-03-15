namespace LimonikOne.Modules.Product.Application.Items.Lookups.Certifications.GetAll;

public sealed record CertificationDto(
    int Id,
    string Name,
    string Label,
    string? ShortName,
    string? Description
);
