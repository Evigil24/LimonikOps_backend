namespace LimonikOne.Modules.Product.Api.Controllers.Products.Requests;

public sealed record CreateProductRequest(
    string ItemNumber,
    string PrimaryName,
    string SearchName,
    int VarietyId,
    int HandlingId,
    int CertificationId,
    int StageId
);
