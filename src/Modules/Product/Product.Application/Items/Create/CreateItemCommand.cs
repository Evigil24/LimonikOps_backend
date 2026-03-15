using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Items.Create;

public sealed record CreateItemCommand(
    string ItemNumber,
    string PrimaryName,
    string SearchName,
    int VarietyId,
    int HandlingId,
    int CertificationId,
    int StageId
) : ICommand<Guid>;
