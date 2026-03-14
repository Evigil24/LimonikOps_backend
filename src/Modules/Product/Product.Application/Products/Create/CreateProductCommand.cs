using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Products.Create;

public sealed record CreateProductCommand(
    string ItemNumber,
    string PrimaryName,
    string SearchName,
    int VarietyId,
    int HandlingId,
    int CertificationId,
    int StageId
) : ICommand<Guid>;
