namespace LimonikOne.Modules.Product.Api.Controllers.Items.Requests;

public sealed record CreateItemRequest(
    string ItemNumber,
    string PrimaryName,
    string SearchName,
    int VarietyId,
    int HandlingId,
    int CertificationId,
    int StageId
);
